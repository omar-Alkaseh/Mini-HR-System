using Application.Common.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Models;
using Domain.Services;

namespace Application.Services;

public sealed class EmployeeLeaveBalanceService(IUnitOfWork unitOfWork) : IEmployeeLeaveBalanceService
{
    public async Task<int> CreateEmployeeLeaveBalanceAsync(int employeeId,
        CancellationToken cancellationToken = default)
    {
        if (employeeId <= 0)
            throw new BadRequestException("Invalid employee id.");

        var employee = await unitOfWork.EmployeeRepository.
            GetEmployeeByIdAsync(employeeId, cancellationToken);

        if (employee is null)
            throw new NotFoundException("Employee is not found.");

        var isEmployeeHasLeaveYear = await unitOfWork.EmployeeLeaveBalanceRepository.IsEmployeeHasLeaveYear(
            employeeId, cancellationToken);

        if (isEmployeeHasLeaveYear)
            throw new ConflictException($"Employee {employee.FullName} Already has a Leave year.");

        var nowUtc = DateTime.UtcNow;

        var leaveYear = (short)nowUtc.Year;

        var entitledDays = EmployeeEntitledDays.CalculateEntitledDays(employee.HireDate, leaveYear);

        var employeeLeaveBalance = EmployeeLeaveBalance.Create(
            employee,
            leaveYear,
            entitledDays,
            nowUtc);

        await unitOfWork.EmployeeLeaveBalanceRepository.AddEmployeeLeaveBalanceAsync(
            employeeLeaveBalance,
            cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return employeeLeaveBalance.BalanceId;
    }

    public async Task<EmployeeLeaveBalanceResult?> GetEmployeeLeaveBalanceAsync(
        int employeeId,
        short leaveYear,
        CancellationToken cancellationToken = default)
    {
        if (employeeId <= 0)
            throw new BadRequestException("Invalid Employee Id.");

        var currentYear = (short)DateTime.UtcNow.Year;

        if (leaveYear > currentYear || leaveYear < 1900)
            throw new BadRequestException("Invalid Leave year.");

        var employeeLeaveBalance = await unitOfWork.EmployeeLeaveBalanceRepository.
            GetEmployeeLeaveBalanceAsync(employeeId, leaveYear, cancellationToken);

        if (employeeLeaveBalance is null)
            throw new NotFoundException($"There is not leave balance for employee id: {employeeId}");

        var remainingDays = employeeLeaveBalance.EntitledDays - employeeLeaveBalance.UsedDays;

        return new EmployeeLeaveBalanceResult(
            employeeLeaveBalance.EmployeeId,
            employeeLeaveBalance.Employee.FullName,
            employeeLeaveBalance.Employee.HireDate,
            employeeLeaveBalance.LeaveYear,
            employeeLeaveBalance.EntitledDays,
            employeeLeaveBalance.UsedDays,
            remainingDays,
            employeeLeaveBalance.CreatedAt);
    }
}
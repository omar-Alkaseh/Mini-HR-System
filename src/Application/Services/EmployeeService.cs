using Application.Common.Exceptions;
using Application.Common.Pagination;
using Application.Common.Validation;
using Application.DTOs.Employee;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Models;
using Domain.Services;
using FluentValidation;

namespace Application.Services;

public sealed class EmployeeService(
    IUnitOfWork unitOfWork,
    IValidator<CreateEmployeeDto> createEmployeeDtoValidator)
    : IEmployeeService
{
    public async Task<int> CreateEmployeeAsync(CreateEmployeeDto dto, CancellationToken cancellationToken = default)
    {
        var validationResult = await createEmployeeDtoValidator.ValidateAsync(dto, cancellationToken);
        validationResult.ThrowIfValidationFails();

        var nowUtc = DateTime.UtcNow;

        var employee = Employee.Create(
            dto.FullName,
            dto.HireDate,
            nowUtc,
            dto.CreatedByUserId);

        await unitOfWork.EmployeeRepository.CreateEmployeeAsync(employee, cancellationToken);

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

        return employee.EmployeeId;
    }

    public async Task<EmployeeResult?> GetEmployeeWithFullInfoByIdAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        if (employeeId <= 0)
            throw new BadRequestException("Invalid Employee Id.");

        var employee = await unitOfWork.EmployeeRepository.
            GetEmployeeWithFullInfoByIdAsync(employeeId, cancellationToken);

        if (employee is null)
            throw new NotFoundException("Employee is not found.");

        return new EmployeeResult(
            employee.EmployeeId,
            employee.FullName,
            employee.HireDate,
            employee.LeaveYear,
            employee.EntitledDays,
            employee.UsedDays,
            employee.IsActive,
            employee.CreatedByAdminEmail);
    }

    public async Task<PagedResult<EmployeeResult>> GetEmployeesAsync(
        PaginationParameters paginationParameters,
        CancellationToken cancellationToken = default) =>
        await unitOfWork.EmployeeRepository.GetEmployeesAsync(paginationParameters, cancellationToken);
}
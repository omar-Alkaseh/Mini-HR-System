using Application.Common.Exceptions;
using Application.Common.Pagination;
using Application.Common.Validation;
using Application.DTOs.EmployeeLeaves;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Models;
using FluentValidation;

namespace Application.Services;

public sealed class EmployeeLeaveService(
    IUnitOfWork unitOfWork,
    IValidator<CreateEmployeeLeavesDto> createEmployeeLeaveValidator)
    : IEmployeeLeaveService
{
    public async Task<int> CreateEmployeeLeaveAsync(
        int employeeId,
        CreateEmployeeLeavesDto dto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await createEmployeeLeaveValidator.ValidateAsync(dto, cancellationToken);
        validationResult.ThrowIfValidationFails();

        var currentYear = (short)dto.StartDate.Year;

        var employeeLeaveBalance = await unitOfWork.EmployeeLeaveBalanceRepository.
            GetEmployeeLeaveBalanceAsync(
                employeeId, 
                currentYear,
                cancellationToken);

        if (employeeLeaveBalance is null)
            throw new NotFoundException($"Employee with Id: {employeeId} has not Leave year.");

        if (await unitOfWork.EmployeeLeaveRepository.HasOverLappingLeaveDateAsync(
            employeeId,
            dto.StartDate,
            dto.EndDate,
            cancellationToken))
                throw new ConflictException($"Employee with Id: {employeeId} has already request leave date in the same period.");

        var daysOfLeaveRequest = EmployeeLeave.CalculateLeaveDays(dto.StartDate, dto.EndDate);

        var daysOfLeaveHas = employeeLeaveBalance.EntitledDays - employeeLeaveBalance.UsedDays;

        if (daysOfLeaveRequest > daysOfLeaveHas)
            throw new BadRequestException("Days of leave request must be less or equal the days of has.");

        var utcNow = DateTime.UtcNow;

        var employeeLeave = EmployeeLeave.Create(
            employeeId,
            dto.StartDate,
            dto.EndDate,
            daysOfLeaveRequest,
            utcNow,
            dto.RecordedByUserId);

        employeeLeaveBalance.UseDays(daysOfLeaveRequest, utcNow);

        await unitOfWork.EmployeeLeaveRepository.AddEmployeeLeaveAsync(employeeLeave, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return employeeLeave.EmployeeLeaveId;
    }

    public async Task<PagedResult<EmployeeLeavesResult>> GetEmployeeLeavesAsync(
        PaginationParameters paginationParameters,
        CancellationToken cancellationToken = default) =>
            await unitOfWork.EmployeeLeaveRepository.GetEmployeeLeavesAsync(
                paginationParameters,
                cancellationToken);
}
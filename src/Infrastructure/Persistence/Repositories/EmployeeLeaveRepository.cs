using Application.Common.Pagination;
using Application.Interfaces.Repositories;
using Application.Results;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class EmployeeLeaveRepository(MiniHrDbContext context) : IEmployeeLeaveRepository
{
    public async Task AddEmployeeLeaveAsync(
        EmployeeLeave employeeLeave,
        CancellationToken cancellationToken = default) =>
            await context.EmployeeLeaves.AddAsync(employeeLeave, cancellationToken);

    public async Task<bool> CanEmployeeLeaveRequestAsync(
        int employeeId,
        decimal entitledDays,
        short leaveYear,
        CancellationToken cancellationToken = default) =>
            await context.EmployeeLeaveBalanced.AnyAsync(
                e => e.EmployeeId == employeeId && 
                e.LeaveYear == leaveYear &&
                e.EntitledDays - e.UsedDays >= entitledDays, cancellationToken);

    public async Task<PagedResult<EmployeeLeavesResult>> GetEmployeeLeavesAsync(
        PaginationParameters paginationParameters,
        CancellationToken cancellationToken = default)
    {
        var query = context.EmployeeLeaves.AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await 
            query
            .OrderBy(e => e.EmployeeLeaveId)
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .Select(e => new EmployeeLeavesResult(
                        e.EmployeeId,
                        e.Employee.FullName,
                        e.StartDate,
                        e.EndDate,
                        e.Days,
                        e.Employee.CreatedByUser.Email,
                        e.CreatedAt
                    ))
            .ToListAsync(cancellationToken);

        return new PagedResult<EmployeeLeavesResult>(
            items,
            paginationParameters.PageNumber,
            paginationParameters.PageSize,
            totalCount);
    }

    public async Task<bool> HasOverLappingLeaveDateAsync(
        int employeeId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken) =>
            await context.EmployeeLeaves
                .AnyAsync(e => e.EmployeeId == employeeId &&
                          e.StartDate <= endDate &&
                          e.EndDate >= startDate,
                          cancellationToken);
}
using Application.Common.Pagination;
using Application.Interfaces.Repositories;
using Application.Results;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class EmployeeRepository(MiniHrDbContext context) : IEmployeeRepository
{
    public async Task CreateEmployeeAsync(Employee employee, CancellationToken cancellationToken = default) =>
        await context.AddAsync(employee, cancellationToken);

    public async Task<EmployeeResult?> GetEmployeeWithFullInfoByIdAsync(int employeeId, CancellationToken cancellationToken = default) =>
        await context.EmployeeLeaveBalanced
            .Where(e => e.EmployeeId == employeeId)
            .Select(e => new EmployeeResult(
                e.EmployeeId,
                e.Employee.FullName,
                e.Employee.HireDate,
                e.LeaveYear,
                e.EntitledDays,
                e.UsedDays,
                e.Employee.IsActive,
                e.Employee.CreatedByUser.Email
            ))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Employee?> GetEmployeeByIdAsync(int employeeId, CancellationToken cancellationToken = default) =>
        await context.Employees
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId, cancellationToken);

    public async Task<PagedResult<EmployeeResult>> GetEmployeesAsync(
        PaginationParameters paginationParameters,
        CancellationToken cancellationToken = default)
    {
        var query = context.EmployeeLeaveBalanced.AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await 
            query
            .OrderBy(e => e.EmployeeId)
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .Select(e => new EmployeeResult(
                        e.EmployeeId,
                        e.Employee.FullName,
                        e.Employee.HireDate,
                        e.LeaveYear,
                        e.EntitledDays,
                        e.UsedDays,
                        e.Employee.IsActive,
                        e.Employee.CreatedByUser.Email
                    ))
            .ToListAsync(cancellationToken);

        return new PagedResult<EmployeeResult>(
            items,
            paginationParameters.PageNumber,
            paginationParameters.PageSize,
            totalCount);
    }
}
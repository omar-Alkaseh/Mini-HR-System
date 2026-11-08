using Application.Common.Pagination;
using Application.Results;
using Domain.Models;

namespace Application.Interfaces.Repositories;

public interface IEmployeeLeaveRepository
{
    Task AddEmployeeLeaveAsync(EmployeeLeave employeeLeave, CancellationToken cancellationToken = default);
    Task<PagedResult<EmployeeLeavesResult>> GetEmployeeLeavesAsync(
        PaginationParameters paginationParameters,
        CancellationToken cancellationToken = default);
    Task<bool> CanEmployeeLeaveRequestAsync(
        int employeeId,
        decimal entitledDays,
        short leaveYear,
        CancellationToken cancellationToken = default);

    Task<bool> HasOverLappingLeaveDateAsync(
        int employeeId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);
}
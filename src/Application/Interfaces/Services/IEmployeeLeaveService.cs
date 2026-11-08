using Application.Common.Pagination;
using Application.DTOs.EmployeeLeaves;
using Application.Results;

namespace Application.Interfaces.Services;

public interface IEmployeeLeaveService
{
    Task<int> CreateEmployeeLeaveAsync(int employeeId,
        CreateEmployeeLeavesDto dto,
        CancellationToken cancellationToken = default);

    Task<PagedResult<EmployeeLeavesResult>> GetEmployeeLeavesAsync(
        PaginationParameters paginationParameters,
        CancellationToken cancellationToken = default);
}
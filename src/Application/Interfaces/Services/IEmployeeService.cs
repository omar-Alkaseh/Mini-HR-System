using Application.Common.Pagination;
using Application.DTOs.Employee;
using Application.Results;

namespace Application.Interfaces.Services;

public interface IEmployeeService
{
    Task<int> CreateEmployeeAsync(CreateEmployeeDto dto, CancellationToken cancellationToken = default);
    Task<EmployeeResult?> GetEmployeeWithFullInfoByIdAsync(int employeeId, CancellationToken cancellationToken = default);
    Task<PagedResult<EmployeeResult>> GetEmployeesAsync(
        PaginationParameters paginationParameters,
        CancellationToken cancellationToken = default);
}
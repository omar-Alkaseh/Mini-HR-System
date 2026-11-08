using Application.Common.Pagination;
using Application.Results;
using Domain.Models;

namespace Application.Interfaces.Repositories;

public interface IEmployeeRepository
{
    Task CreateEmployeeAsync(Employee employee, CancellationToken cancellationToken = default);
    Task<EmployeeResult?> GetEmployeeWithFullInfoByIdAsync(int employeeId, CancellationToken cancellationToken = default);
    Task<Employee?> GetEmployeeByIdAsync(int employeeId, CancellationToken cancellationToken = default);
    Task<PagedResult<EmployeeResult>> GetEmployeesAsync(
        PaginationParameters paginationParameters,
        CancellationToken cancellationToken = default);
}
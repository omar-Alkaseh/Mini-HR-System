using Domain.Models;

namespace Application.Interfaces.Repositories;

public interface IEmployeeLeaveBalanceRepository
{
    Task<EmployeeLeaveBalance?> GetEmployeeLeaveBalanceAsync(
        int employeeId, short leaveYear, CancellationToken cancellationToken = default);

    Task AddEmployeeLeaveBalanceAsync(
        EmployeeLeaveBalance balance,
        CancellationToken cancellationToken = default);

    Task<bool> IsEmployeeHasLeaveYear(int employeeId, CancellationToken cancellationToken = default);
}
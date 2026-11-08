using Application.Results;

namespace Application.Interfaces.Services;

public interface IEmployeeLeaveBalanceService
{
    Task<EmployeeLeaveBalanceResult?> GetEmployeeLeaveBalanceAsync(
        int employeeId,
        short leaveYear,
        CancellationToken cancellationToken = default);
    Task<int> CreateEmployeeLeaveBalanceAsync(
        int employeeId,
        CancellationToken cancellationToken = default);
}
namespace Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IEmployeeRepository EmployeeRepository { get; }
    IEmployeeLeaveBalanceRepository EmployeeLeaveBalanceRepository { get; }
    IEmployeeLeaveRepository EmployeeLeaveRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
using Application.Interfaces.Repositories;

namespace Infrastructure.Persistence.Repositories;

public class UnitOfWork(
    MiniHrDbContext context,
    IUserRepository userRepository,
    IEmployeeRepository employeeRepository,
    IEmployeeLeaveRepository employeeLeaveRepository,
    IEmployeeLeaveBalanceRepository employeeLeaveBalanceRepository) : IUnitOfWork
{
    public IUserRepository UserRepository => userRepository;
    public IEmployeeRepository EmployeeRepository => employeeRepository;

    public IEmployeeLeaveBalanceRepository EmployeeLeaveBalanceRepository => employeeLeaveBalanceRepository;

    public IEmployeeLeaveRepository EmployeeLeaveRepository => employeeLeaveRepository;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}
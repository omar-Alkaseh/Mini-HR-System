using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class EmployeeLeaveBalanceRepository(MiniHrDbContext context) : IEmployeeLeaveBalanceRepository
{
    public async Task AddEmployeeLeaveBalanceAsync(
        EmployeeLeaveBalance balance,
        CancellationToken cancellationToken = default) =>
            await context.AddAsync(balance, cancellationToken);


    public async Task<EmployeeLeaveBalance?> GetEmployeeLeaveBalanceAsync(
        int employeeId,
        short leaveYear,
        CancellationToken cancellationToken = default) =>
            await context.EmployeeLeaveBalanced.
            Include(e => e.Employee).
            FirstOrDefaultAsync(
                e => e.EmployeeId == employeeId &&
                e.LeaveYear == leaveYear, cancellationToken);

    public async Task<bool> IsEmployeeHasLeaveYear(int employeeId, CancellationToken cancellationToken = default) =>
        await context.EmployeeLeaveBalanced.AnyAsync(
            e => e.EmployeeId == employeeId && e.LeaveYear == DateTime.UtcNow.Year, cancellationToken);
}
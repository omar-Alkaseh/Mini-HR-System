using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(MiniHrDbContext context) : IUserRepository
{
    public async Task AddAsync(User Account, CancellationToken ct) =>
        await context.Users.AddAsync(Account, ct);

    public async Task<bool> ExistsByEmailAsync(string Email, CancellationToken ct) =>
        await context.Users.AnyAsync(a => a.Email == Email, ct);

    public async Task<User?> GetByEmailAsync(string Email, CancellationToken ct) =>
        await context.Users.FirstOrDefaultAsync(a => a.Email == Email, ct);

    public async Task<User?> GetByIdAsync(int AdminId, CancellationToken ct) =>
        await context.Users.FirstOrDefaultAsync(a => a.UserId == AdminId, ct);
}
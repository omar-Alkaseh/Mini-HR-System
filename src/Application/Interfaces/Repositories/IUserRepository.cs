using Domain.Models;

namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(string Email, CancellationToken ct);
    Task<User?> GetByEmailAsync(string Email, CancellationToken ct);
    Task<User?> GetByIdAsync(int UserId, CancellationToken ct);

    Task AddAsync(User User, CancellationToken ct);
}
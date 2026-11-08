using Application.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Services;

public sealed class PasswordHasherService : IPasswordHasherService
{
    private readonly PasswordHasher<object> _hasher = new();
    public string HashPassword(string password)
    {
        return _hasher.HashPassword(new object(), password);
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        var result = _hasher.VerifyHashedPassword(
            new object(),
            hashedPassword,
            password);

        return result != PasswordVerificationResult.Failed;
    }
}
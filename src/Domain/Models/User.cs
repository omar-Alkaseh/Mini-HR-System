using Domain.Common.Exceptions;

namespace Domain.Models;

public sealed class User
{
    public int UserId { get; private set; }

    public string FullName { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string PasswordHash { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    private User() { }

    public static User Create(
        string fullName,
        string email,
        string password,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("email is required.");

        if (string.IsNullOrWhiteSpace(password))
            throw new DomainException("password is required.");

        return new User
        {
            FullName = fullName.Trim(),
            Email = email.Trim(),
            PasswordHash = password,
            IsActive = true,
            CreatedAt = nowUtc
        };
    }

    public void Activate(DateTime nowUtc)
    {
        IsActive = true;
        UpdatedAt = nowUtc;
    }

    public void DeActivate(DateTime nowUtc)
    {
        IsActive = false;
        UpdatedAt = nowUtc;
    }
}
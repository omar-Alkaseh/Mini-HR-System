using Domain.Common.Exceptions;

namespace Domain.Models;

public sealed class Employee
{
    public int EmployeeId { get; private set; }

    public string FullName { get; private set; } = null!;

    public DateOnly HireDate { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public int CreatedByUserId { get; private set; }

    public User CreatedByUser { get; private set; } = null!;

    private Employee() { }

    public static Employee Create(
        string fullName,
        DateOnly hireDate,
        DateTime nowUtc,
        int createdByAdminId)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("full name is required.");

        if (hireDate == default)
            throw new DomainException("Hire date is required.");

        if (hireDate > DateOnly.FromDateTime(nowUtc))
            throw new DomainException("Hire date cannot be in future.");

        if (createdByAdminId <= 0)
            throw new DomainException("Invalid admin ID.");

        return new Employee
        {
            FullName = fullName.Trim(),
            HireDate = hireDate,
            IsActive = true,
            CreatedAt = nowUtc,
            CreatedByUserId = createdByAdminId
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
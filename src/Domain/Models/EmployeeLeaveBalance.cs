using Domain.Common.Exceptions;

namespace Domain.Models;

public sealed class EmployeeLeaveBalance
{
    public int BalanceId { get; private set; }

    public int EmployeeId { get; private set; }

    public short LeaveYear { get; private set; }

    public decimal EntitledDays { get; private set; }

    public int UsedDays { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public Employee Employee { get; private set; } = null!;

    private EmployeeLeaveBalance() { }

    public static EmployeeLeaveBalance Create(
        Employee employee,
        short leaveYear,
        decimal entitledDays,
        DateTime nowUtc)
    {
        if (employee is null)
            throw new DomainException("Invalid employee object.");

        if (leaveYear != nowUtc.Year)
            throw new DomainException($"Leave year must be {nowUtc.Year}.");

        if (entitledDays is < 0 or > 30)
            throw new DomainException("Entitled days must be between 0 and 30.");

        return new EmployeeLeaveBalance
        {
            Employee = employee,
            LeaveYear = leaveYear,
            EntitledDays = entitledDays,
            UsedDays = 0,
            CreatedAt = nowUtc
        };
    }

    public void UseDays(int days, DateTime nowUtc)
    {
        if (days <= 0)
        throw new DomainException(
            "Used days must be greater than zero.");

        if (UsedDays + days > EntitledDays)
            throw new DomainException("Insufficient annual leave balance.");

        UsedDays += days;
        UpdatedAt = nowUtc;
    }
}
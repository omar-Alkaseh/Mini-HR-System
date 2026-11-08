namespace Application.Results;

public sealed record EmployeeLeaveBalanceResult(
    int EmployeeId,
    string FullName,
    DateOnly HireDate,
    short LeaveYear,
    decimal EntitledDays,
    int UsedDays,
    decimal RemainingDays,
    DateTime CreatedAt);
namespace Application.Results;

public sealed record EmployeeResult(
    int EmployeeId,
    string FullName,
    DateOnly HireDate,
    short LeaveYear,
    decimal EntitledDays,
    int UsedDays,
    bool IsActive,
    string CreatedByAdminEmail);
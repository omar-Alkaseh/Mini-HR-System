namespace Application.Results;

public sealed record EmployeeLeavesResult(
    int EmployeeId,
    string EmployeeName,
    DateOnly StartDate,
    DateOnly EndDate,
    int Days,
    string CreatedByAdminName,
    DateTime CreatedAt);
namespace Application.DTOs.EmployeeLeaves;

public sealed record CreateEmployeeLeavesDto(
    DateOnly StartDate,
    DateOnly EndDate,
    int RecordedByUserId);
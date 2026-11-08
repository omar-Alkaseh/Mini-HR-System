namespace Application.DTOs.Employee;

public sealed record CreateEmployeeDto(
    string FullName,
    DateOnly HireDate,
    int CreatedByUserId);
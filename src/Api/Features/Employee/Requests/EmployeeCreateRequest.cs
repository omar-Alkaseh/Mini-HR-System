namespace Api.Features.Employee.Requests;

public sealed record EmployeeCreateRequest(
    string FullName,
    DateOnly HireDate);
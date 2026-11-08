using Api.Features.Employee.Requests;
using Application.DTOs.Employee;

namespace Api.Features.Employee.Mapping;

public static class EmployeeMapping
{
    public static CreateEmployeeDto ToCreateDto(this EmployeeCreateRequest request, int userId) =>
        new (request.FullName, request.HireDate, userId);
}
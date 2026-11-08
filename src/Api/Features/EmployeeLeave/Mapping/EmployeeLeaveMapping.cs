using Api.Features.EmployeeLeave.Requests;
using Application.DTOs.EmployeeLeaves;

namespace Api.Features.EmployeeLeave.Mapping;

public static class EmployeeLeaveMapping
{
    public static CreateEmployeeLeavesDto ToDto(this CreateEmployeeLeaveRequest request, int userId) =>
        new (request.StartDate, request.EndDate, userId);
}
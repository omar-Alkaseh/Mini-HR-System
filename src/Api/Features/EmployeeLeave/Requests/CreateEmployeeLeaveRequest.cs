namespace Api.Features.EmployeeLeave.Requests;

public sealed record CreateEmployeeLeaveRequest(
    DateOnly StartDate,
    DateOnly EndDate);
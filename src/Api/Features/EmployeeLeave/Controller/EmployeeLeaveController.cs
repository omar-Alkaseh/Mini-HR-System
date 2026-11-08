using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Api.Features.EmployeeLeave.Mapping;
using Api.Features.EmployeeLeave.Requests;
using Application.Common.Pagination;
using Application.Interfaces.Services;
using Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.EmployeeLeave.Controller;

[ApiController]
[Route("api/v1/employee/leaves")]
[Authorize]
public sealed class EmployeeLeaveController(IEmployeeLeaveService leaveService) : ControllerBase
{
    [HttpPost("{employeeId:int}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<int>> CreateEmployeeLeaveAsync(
        int employeeId,
        CreateEmployeeLeaveRequest request,
        CancellationToken cancellationToken)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        return await leaveService.CreateEmployeeLeaveAsync(employeeId, request.ToDto(userId), cancellationToken);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PagedResult<EmployeeLeavesResult>>> GetEmployeeLeavesAsync(
        [FromQuery, Range(1, int.MaxValue)]int pageNumber = 1,
        [FromQuery, Range(1, 100)]int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var paginationParameters = new PaginationParameters
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return await leaveService.GetEmployeeLeavesAsync(paginationParameters, cancellationToken);
    }
}
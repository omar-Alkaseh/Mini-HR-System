using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Api.Features.Employee.Mapping;
using Api.Features.Employee.Requests;
using Application.Common.Pagination;
using Application.Interfaces.Services;
using Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Employee.Controller;

[ApiController]
[Route("api/v1/employee")]
[Authorize]
public sealed class EmployeeController(IEmployeeService employeeService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<int>> Create(EmployeeCreateRequest request, CancellationToken cancellationToken)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        return await employeeService.CreateEmployeeAsync(request.ToCreateDto(userId), cancellationToken);
    }

    [HttpGet("{employeeId:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeResult?>> GetEmployeeWithFullInfoById(
        int employeeId,
        CancellationToken cancellationToken)
    {
        return await employeeService.GetEmployeeWithFullInfoByIdAsync(employeeId, cancellationToken);
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PagedResult<EmployeeResult>>> GetEmployees(
        [FromQuery, Range(1, int.MaxValue)]int pageNumber = 1,
        [FromQuery, Range(1, 100)]int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var paginationParameters = new PaginationParameters
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return await employeeService.GetEmployeesAsync(paginationParameters, cancellationToken);
    }
}
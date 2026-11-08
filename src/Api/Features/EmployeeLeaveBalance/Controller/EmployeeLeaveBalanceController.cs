using Application.Interfaces.Services;
using Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.EmployeeLeaveBalance.Controller;

[ApiController]
[Route("api/v1/employee/{employeeId:int}/leave-balances")]
[Authorize]
public sealed class EmployeeLeaveBalanceController(IEmployeeLeaveBalanceService balanceService) 
    : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<int>> CreateEmployeeLeaveBalance(
        int employeeId,
        CancellationToken cancellationToken)
    {
        return await balanceService.CreateEmployeeLeaveBalanceAsync(employeeId, cancellationToken);
    }

    [HttpGet("{leaveYear:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EmployeeLeaveBalanceResult?>> GetEmployeeLeaveBalance(
        int employeeId,
        short leaveYear,
        CancellationToken cancellationToken)
    {
        return await balanceService.GetEmployeeLeaveBalanceAsync(employeeId, leaveYear, cancellationToken);
    }
}
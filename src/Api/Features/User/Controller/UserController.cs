using System.Security.Claims;
using Api.Features.User.Mapping;
using Api.Features.User.Requests;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.User.Controller;

[ApiController]
[Route("api/v1/auth/admin")]
public sealed class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<int>> Register(UserRegisterRequest request, CancellationToken cancellationToken)
    {
        return await userService.UserRegisterAsync(request.ToRegisterDto(), cancellationToken);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Login(UserLoginRequest request, CancellationToken cancellationToken)
    {
        var result = await userService.UserLoginAsync(request.ToLoginDto(), cancellationToken);

        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, result.UserId.ToString()),
            new (ClaimTypes.Name, result.FullName),
            new (ClaimTypes.Role, "Admin")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var claimPrinciple = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimPrinciple);

        return Ok(new {Message = "Login Successfully."});
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return NoContent();
    }
}
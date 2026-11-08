namespace Api.Features.User.Requests;

public sealed record UserLoginRequest(
    string Email,
    string Password);
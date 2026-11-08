namespace Api.Features.User.Requests;

public sealed record UserRegisterRequest(
    string FullName,
    string Email,
    string Password);
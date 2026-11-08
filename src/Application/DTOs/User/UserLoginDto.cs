namespace Application.DTOs.User;

public sealed record UserLoginDto(
    string Email,
    string Password);
namespace Application.DTOs.User;

public sealed record UserRegisterDto(
    string FullName,
    string Email,
    string Password);
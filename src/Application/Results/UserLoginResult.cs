namespace Application.Results;

public sealed record UserLoginResult(
    int UserId,
    string FullName);
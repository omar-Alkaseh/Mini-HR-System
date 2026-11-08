using Application.DTOs.User;
using Application.Results;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<UserLoginResult> UserLoginAsync(UserLoginDto dto, CancellationToken cancellationToken = default);
    Task<int> UserRegisterAsync(UserRegisterDto dto, CancellationToken cancellationToken = default);
}
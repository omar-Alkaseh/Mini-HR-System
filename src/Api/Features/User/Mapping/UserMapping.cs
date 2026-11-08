using Api.Features.Employee.Requests;
using Api.Features.User.Requests;
using Application.DTOs.User;

namespace Api.Features.User.Mapping;

public static class UserMapping
{
    public static UserRegisterDto ToRegisterDto(this UserRegisterRequest request) =>
        new (request.FullName, request.Email, request.Password);

    public static UserLoginDto ToLoginDto(this UserLoginRequest request) =>
        new (request.Email, request.Password);
}
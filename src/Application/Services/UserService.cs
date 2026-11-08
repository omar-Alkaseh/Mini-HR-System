using Application.Common.Exceptions;
using Application.Common.Validation;
using Application.DTOs.User;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Models;
using FluentValidation;

namespace Application.Services;

public sealed class UserService(
    IUnitOfWork unitOfWork,
    IValidator<UserLoginDto> userLoginValidator,
    IValidator<UserRegisterDto> userRegisterValidator,
    IPasswordHasherService passwordHasherService) : IUserService
{
    public async Task<UserLoginResult> UserLoginAsync(UserLoginDto dto, CancellationToken cancellationToken = default)
    {
        var validationResult = await userLoginValidator.ValidateAsync(dto, cancellationToken);
        validationResult.ThrowIfValidationFails();

        var user = await unitOfWork.UserRepository.GetByEmailAsync(dto.Email.Trim(), cancellationToken);

        if (user is null)
            throw new UnAuthorizeException("Invalid Email or Password.");

        if (!passwordHasherService.VerifyPassword(user.PasswordHash, dto.Password))
            throw new UnAuthorizeException("Invalid Email or Password.");

        if (!user.IsActive)
            throw new UnAuthorizeException("Account is inactive.");

        return new UserLoginResult(
            user.UserId,
            user.FullName);
    }

    public async Task<int> UserRegisterAsync(UserRegisterDto dto, CancellationToken cancellationToken = default)
    {
        var validationResult = await userRegisterValidator.ValidateAsync(dto, cancellationToken);
        validationResult.ThrowIfValidationFails();

        var isEmailExist = await unitOfWork.UserRepository.ExistsByEmailAsync(dto.Email, cancellationToken);

        if (isEmailExist)
            throw new ConflictException("Email is already exist.");

        var passwordHasher = passwordHasherService.HashPassword(dto.Password);

        var user = User.Create(
            dto.FullName,
            dto.Email,
            passwordHasher,
            DateTime.UtcNow);

        await unitOfWork.UserRepository.AddAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.UserId;
    }
}
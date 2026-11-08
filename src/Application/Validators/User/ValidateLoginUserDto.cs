using Application.DTOs.User;
using FluentValidation;

namespace Application.Validators.User;

public sealed class ValidateLoginUserDto : AbstractValidator<UserLoginDto>
{
    public ValidateLoginUserDto()
    {
        RuleFor(x => x.Email)
        .Cascade(CascadeMode.Stop)
        .NotEmpty()
        .WithMessage("Email is required.")
        .EmailAddress()
        .WithMessage("Invalid email.");

        RuleFor(x => x.Password)
        .NotEmpty()
        .WithMessage("Password is required.");
    }
}
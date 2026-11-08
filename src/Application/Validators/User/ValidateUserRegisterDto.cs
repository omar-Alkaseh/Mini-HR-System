using Application.DTOs.User;
using FluentValidation;

namespace Application.Validators.User;

public sealed class ValidateUserRegisterDto : AbstractValidator<UserRegisterDto>
{
    public ValidateUserRegisterDto()
    {
        RuleFor(x => x.FullName)
        .Cascade(CascadeMode.Stop)
        .NotEmpty()
        .WithMessage("Name is required.")
        .MaximumLength(100)
        .WithMessage("name at must a 100 Character");
        
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
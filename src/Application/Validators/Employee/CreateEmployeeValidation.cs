using Application.DTOs.Employee;
using FluentValidation;

namespace Application.Validators.Employee;

public sealed class CreateEmployeeValidation : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeValidation()
    {
        RuleFor(x => x.FullName)
        .Cascade(CascadeMode.Stop)
        .NotEmpty()
        .WithMessage("Name is required.")
        .MaximumLength(100)
        .WithMessage("name at must a 100 Character");

        RuleFor(x => x.HireDate)
        .Cascade(CascadeMode.Stop)
        .NotEmpty()
        .WithMessage("Hire date is required.")
        .Must(date => date <= DateOnly.FromDateTime(DateTime.UtcNow))
        .WithMessage("Hire date cannot be in the future.");
    }
}
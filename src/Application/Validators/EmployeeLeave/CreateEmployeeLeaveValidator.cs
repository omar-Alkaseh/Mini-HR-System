using Application.DTOs.EmployeeLeaves;
using FluentValidation;

namespace Application.Validators.EmployeeLeave;

public sealed class CreateEmployeeLeaveValidator : AbstractValidator<CreateEmployeeLeavesDto>
{
    public CreateEmployeeLeaveValidator()
    {
        RuleFor(x => x.StartDate)
        .NotEmpty()
        .WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
        .Cascade(CascadeMode.Stop)
        .NotEmpty()
        .WithMessage("End date is required.")
        .GreaterThanOrEqualTo(x => x.StartDate)
        .WithMessage("End date must be greater than or equal to start date.")
        .Must((dto, endDate) => 
            dto.StartDate != default &&
            endDate.Year == dto.StartDate.Year)
        .WithMessage("Start date and end date must be in the same year.");
    }
}
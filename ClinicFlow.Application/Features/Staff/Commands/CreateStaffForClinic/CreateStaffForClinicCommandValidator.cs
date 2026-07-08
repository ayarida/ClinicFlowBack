using FluentValidation;

namespace ClinicFlow.Application.Features.Staff.Commands.CreateStaffForClinic;

public class CreateStaffForClinicCommandValidator : AbstractValidator<CreateStaffForClinicCommand>
{
    public CreateStaffForClinicCommandValidator()
    {
        RuleFor(x => x.ClinicId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(300);
        RuleFor(x => x.Role).IsInEnum();
    }
}

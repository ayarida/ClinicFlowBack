using FluentValidation;

namespace ClinicFlow.Application.Features.Treatments.Commands.CreateTreatment;

public class CreateTreatmentCommandValidator : AbstractValidator<CreateTreatmentCommand>
{
    public CreateTreatmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Treatment name is required.")
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => x.Description is not null);

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Duration must be greater than 0.");

        RuleFor(x => x.FollowUpIntervalDays)
            .GreaterThan(0).WithMessage("Follow-up interval must be greater than 0.")
            .When(x => x.FollowUpIntervalDays is not null);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.")
            .When(x => x.Price is not null);
    }
}

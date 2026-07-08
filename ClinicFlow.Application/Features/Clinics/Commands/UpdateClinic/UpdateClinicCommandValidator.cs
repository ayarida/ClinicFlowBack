using FluentValidation;

namespace ClinicFlow.Application.Features.Clinics.Commands.UpdateClinic;

public class UpdateClinicCommandValidator : AbstractValidator<UpdateClinicCommand>
{
    public UpdateClinicCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(500);
        RuleFor(x => x.LogoUrl).MaximumLength(1000).When(x => x.LogoUrl is not null);
        RuleFor(x => x.SlotDurationMinutes).GreaterThan(0).LessThanOrEqualTo(240);
        RuleFor(x => x.WorkingHoursEnd)
            .GreaterThan(x => x.WorkingHoursStart)
            .WithMessage("Working hours end must be after start.");
    }
}

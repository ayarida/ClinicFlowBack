using FluentValidation;

namespace ClinicFlow.Application.Features.Appointments.Commands.UpdateAppointment;

public class UpdateAppointmentCommandValidator : AbstractValidator<UpdateAppointmentCommand>
{
    public UpdateAppointmentCommandValidator()
    {
        RuleFor(x => x.Notes).MaximumLength(2000).When(x => x.Notes is not null);
        RuleFor(x => x.Treatments).NotEmpty().WithMessage("At least one treatment is required.");
    }
}

using MediatR;

namespace ClinicFlow.Application.Features.Treatments.Commands.DeactivateTreatment;

public record DeactivateTreatmentCommand(Guid Id) : IRequest<Unit>;

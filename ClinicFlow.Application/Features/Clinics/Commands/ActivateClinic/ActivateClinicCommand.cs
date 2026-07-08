using MediatR;

namespace ClinicFlow.Application.Features.Clinics.Commands.ActivateClinic;

public record ActivateClinicCommand(Guid Id) : IRequest<Unit>;

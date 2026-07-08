using MediatR;

namespace ClinicFlow.Application.Features.Clinics.Commands.DeactivateClinic;

public record DeactivateClinicCommand(Guid Id) : IRequest<Unit>;

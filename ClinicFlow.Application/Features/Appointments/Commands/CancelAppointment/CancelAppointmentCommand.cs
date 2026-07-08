using MediatR;

namespace ClinicFlow.Application.Features.Appointments.Commands.CancelAppointment;

public record CancelAppointmentCommand(Guid Id) : IRequest<Unit>;

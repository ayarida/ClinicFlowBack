using MediatR;

namespace ClinicFlow.Application.Features.Appointments.Commands.CompleteAppointment;

public record CompleteAppointmentCommand(Guid Id) : IRequest<Unit>;

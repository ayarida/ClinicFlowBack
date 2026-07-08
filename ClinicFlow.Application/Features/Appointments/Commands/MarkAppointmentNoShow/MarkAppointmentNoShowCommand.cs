using MediatR;

namespace ClinicFlow.Application.Features.Appointments.Commands.MarkAppointmentNoShow;

public record MarkAppointmentNoShowCommand(Guid Id) : IRequest<Unit>;

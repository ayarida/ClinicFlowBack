using ClinicFlow.Application.Features.Appointments.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Appointments.Queries.GetAppointmentById;

public record GetAppointmentByIdQuery(Guid Id) : IRequest<AppointmentDto>;

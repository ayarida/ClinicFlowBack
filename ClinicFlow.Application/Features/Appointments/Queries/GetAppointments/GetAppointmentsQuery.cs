using ClinicFlow.Application.Features.Appointments.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Appointments.Queries.GetAppointments;

public record GetAppointmentsQuery(DateOnly? Date, Guid? CustomerId) : IRequest<IReadOnlyList<AppointmentSummaryDto>>;

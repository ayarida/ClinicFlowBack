using ClinicFlow.Application.Features.Appointments.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Appointments.Queries.GetCalendar;

public record GetCalendarQuery(int Year, int Month) : IRequest<IReadOnlyList<CalendarDayDto>>;

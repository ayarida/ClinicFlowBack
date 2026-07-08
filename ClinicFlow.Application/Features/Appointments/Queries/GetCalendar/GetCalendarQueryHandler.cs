using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Application.Features.Appointments.DTOs;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Appointments.Queries.GetCalendar;

public class GetCalendarQueryHandler(
    IAppointmentRepository appointmentRepository,
    ICurrentClinicService currentClinic
) : IRequestHandler<GetCalendarQuery, IReadOnlyList<CalendarDayDto>>
{
    public async Task<IReadOnlyList<CalendarDayDto>> Handle(GetCalendarQuery request, CancellationToken cancellationToken)
    {
        var appointments = await appointmentRepository.GetByMonthAsync(
            currentClinic.ClinicId, request.Year, request.Month, cancellationToken);

        return appointments
            .GroupBy(a => a.Date)
            .OrderBy(g => g.Key)
            .Select(g => new CalendarDayDto(
                g.Key,
                g.OrderBy(a => a.TimeSlot)
                 .Select(a => new AppointmentSummaryDto(
                     a.Id,
                     a.CustomerId,
                     a.Customer.Name,
                     a.Date,
                     a.TimeSlot,
                     a.Status,
                     a.Treatments.Select(t => t.Treatment.Name).ToList()))
                 .ToList()))
            .ToList();
    }
}

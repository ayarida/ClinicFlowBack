using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Application.Features.Appointments.DTOs;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Appointments.Queries.GetAppointments;

public class GetAppointmentsQueryHandler(
    IAppointmentRepository appointmentRepository,
    ICurrentClinicService currentClinic
) : IRequestHandler<GetAppointmentsQuery, IReadOnlyList<AppointmentSummaryDto>>
{
    public async Task<IReadOnlyList<AppointmentSummaryDto>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var appointments = await appointmentRepository.GetAllAsync(
            currentClinic.ClinicId, request.Date, request.CustomerId, cancellationToken);

        return appointments.Select(a => new AppointmentSummaryDto(
            a.Id,
            a.CustomerId,
            a.Customer.Name,
            a.Date,
            a.TimeSlot,
            a.Status,
            a.Treatments.Select(t => t.Treatment.Name).ToList()
        )).ToList();
    }
}

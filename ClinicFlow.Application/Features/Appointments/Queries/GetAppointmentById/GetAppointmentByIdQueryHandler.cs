using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Application.Features.Appointments.DTOs;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Appointments.Queries.GetAppointmentById;

public class GetAppointmentByIdQueryHandler(
    IAppointmentRepository appointmentRepository,
    ICurrentClinicService currentClinic
) : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto>
{
    public async Task<AppointmentDto> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var a = await appointmentRepository.GetByIdAsync(request.Id, currentClinic.ClinicId, cancellationToken);
        if (a is null)
            throw new NotFoundException(nameof(Appointment), request.Id);

        return new AppointmentDto(
            a.Id,
            a.CustomerId,
            a.Customer.Name,
            a.Date,
            a.TimeSlot,
            a.Status,
            a.Notes,
            a.Treatments.Select(t => new AppointmentTreatmentItemDto(t.TreatmentId, t.Treatment.Name, t.Notes)).ToList(),
            a.CreatedAt);
    }
}

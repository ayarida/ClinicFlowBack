using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Application.Features.Appointments.DTOs;
using ClinicFlow.Application.Features.Reports.DTOs;
using ClinicFlow.Domain.Enums;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Reports.Queries.GetDailyReport;

public class GetDailyReportQueryHandler(
    IAppointmentRepository appointmentRepository,
    ICurrentClinicService currentClinic
) : IRequestHandler<GetDailyReportQuery, DailyReportDto>
{
    public async Task<DailyReportDto> Handle(GetDailyReportQuery request, CancellationToken cancellationToken)
    {
        var appointments = await appointmentRepository.GetAllAsync(
            currentClinic.ClinicId, request.Date, null, cancellationToken);

        var summaries = appointments
            .Select(a => new AppointmentSummaryDto(
                a.Id,
                a.CustomerId,
                a.Customer.Name,
                a.Date,
                a.TimeSlot,
                a.Status,
                a.Treatments.Select(t => t.Treatment.Name).ToList()))
            .ToList();

        return new DailyReportDto(
            Date: request.Date,
            Total: appointments.Count,
            Scheduled: appointments.Count(a => a.Status == AppointmentStatus.Scheduled),
            Completed: appointments.Count(a => a.Status == AppointmentStatus.Completed),
            Cancelled: appointments.Count(a => a.Status == AppointmentStatus.Cancelled),
            NoShow: appointments.Count(a => a.Status == AppointmentStatus.NoShow),
            Appointments: summaries);
    }
}

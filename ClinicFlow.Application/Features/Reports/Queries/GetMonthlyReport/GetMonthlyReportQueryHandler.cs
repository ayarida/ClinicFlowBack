using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Application.Features.Reports.DTOs;
using ClinicFlow.Domain.Enums;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Reports.Queries.GetMonthlyReport;

public class GetMonthlyReportQueryHandler(
    IAppointmentRepository appointmentRepository,
    ICurrentClinicService currentClinic
) : IRequestHandler<GetMonthlyReportQuery, MonthlyReportDto>
{
    public async Task<MonthlyReportDto> Handle(GetMonthlyReportQuery request, CancellationToken cancellationToken)
    {
        var appointments = await appointmentRepository.GetByMonthAsync(
            currentClinic.ClinicId, request.Year, request.Month, cancellationToken);

        var days = appointments
            .GroupBy(a => a.Date)
            .OrderBy(g => g.Key)
            .Select(g => new MonthlyDayStatDto(
                Date: g.Key,
                Total: g.Count(),
                Scheduled: g.Count(a => a.Status == AppointmentStatus.Scheduled),
                Completed: g.Count(a => a.Status == AppointmentStatus.Completed),
                Cancelled: g.Count(a => a.Status == AppointmentStatus.Cancelled),
                NoShow: g.Count(a => a.Status == AppointmentStatus.NoShow)))
            .ToList();

        return new MonthlyReportDto(
            Year: request.Year,
            Month: request.Month,
            Total: appointments.Count,
            TotalScheduled: appointments.Count(a => a.Status == AppointmentStatus.Scheduled),
            TotalCompleted: appointments.Count(a => a.Status == AppointmentStatus.Completed),
            TotalCancelled: appointments.Count(a => a.Status == AppointmentStatus.Cancelled),
            TotalNoShow: appointments.Count(a => a.Status == AppointmentStatus.NoShow),
            Days: days);
    }
}

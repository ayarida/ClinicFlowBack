using ClinicFlow.Application.Features.Appointments.DTOs;

namespace ClinicFlow.Application.Features.Reports.DTOs;

public record DailyReportDto(
    DateOnly Date,
    int Total,
    int Scheduled,
    int Completed,
    int Cancelled,
    int NoShow,
    IReadOnlyList<AppointmentSummaryDto> Appointments);

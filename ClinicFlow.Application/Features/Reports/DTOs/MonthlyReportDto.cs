namespace ClinicFlow.Application.Features.Reports.DTOs;

public record MonthlyDayStatDto(
    DateOnly Date,
    int Total,
    int Scheduled,
    int Completed,
    int Cancelled,
    int NoShow);

public record MonthlyReportDto(
    int Year,
    int Month,
    int Total,
    int TotalScheduled,
    int TotalCompleted,
    int TotalCancelled,
    int TotalNoShow,
    IReadOnlyList<MonthlyDayStatDto> Days);

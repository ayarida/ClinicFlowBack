namespace ClinicFlow.Application.Features.Appointments.DTOs;

public record CalendarDayDto(DateOnly Date, IReadOnlyList<AppointmentSummaryDto> Appointments);

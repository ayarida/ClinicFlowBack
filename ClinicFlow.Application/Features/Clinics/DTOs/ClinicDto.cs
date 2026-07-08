namespace ClinicFlow.Application.Features.Clinics.DTOs;

public record ClinicDto(
    Guid Id,
    string Name,
    string Phone,
    string Address,
    string? LogoUrl,
    TimeOnly WorkingHoursStart,
    TimeOnly WorkingHoursEnd,
    int SlotDurationMinutes,
    bool IsActive);

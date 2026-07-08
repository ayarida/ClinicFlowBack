namespace ClinicFlow.Application.Features.Treatments.DTOs;

public record TreatmentDto(
    Guid Id,
    string Name,
    string? Description,
    int DurationMinutes,
    int? FollowUpIntervalDays,
    decimal? Price,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

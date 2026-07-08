namespace ClinicFlow.Application.Features.Appointments.DTOs;

public record FollowUpSuggestionDto(
    Guid TreatmentId,
    string TreatmentName,
    int FollowUpIntervalDays,
    DateOnly SuggestedDate);

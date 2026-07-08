namespace ClinicFlow.Application.Features.Appointments.DTOs;

public record AppointmentTreatmentItemDto(
    Guid TreatmentId,
    string TreatmentName,
    string? Notes);

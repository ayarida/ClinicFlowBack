using ClinicFlow.Domain.Enums;

namespace ClinicFlow.Application.Features.Appointments.DTOs;

public record AppointmentDto(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    DateOnly Date,
    TimeOnly TimeSlot,
    AppointmentStatus Status,
    string? Notes,
    IReadOnlyList<AppointmentTreatmentItemDto> Treatments,
    DateTime CreatedAt);

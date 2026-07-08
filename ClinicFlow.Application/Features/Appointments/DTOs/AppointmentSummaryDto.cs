using ClinicFlow.Domain.Enums;

namespace ClinicFlow.Application.Features.Appointments.DTOs;

public record AppointmentSummaryDto(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    DateOnly Date,
    TimeOnly TimeSlot,
    AppointmentStatus Status,
    IReadOnlyList<string> TreatmentNames);

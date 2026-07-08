using ClinicFlow.Application.Features.Appointments.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Appointments.Commands.UpdateAppointment;

public record UpdateAppointmentCommand(
    Guid Id,
    DateOnly Date,
    TimeOnly TimeSlot,
    string? Notes,
    List<TreatmentLineItem> Treatments) : IRequest<Unit>;

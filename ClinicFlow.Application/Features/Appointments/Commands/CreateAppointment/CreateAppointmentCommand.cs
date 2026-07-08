using ClinicFlow.Application.Features.Appointments.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Appointments.Commands.CreateAppointment;

public record CreateAppointmentCommand(
    Guid CustomerId,
    DateOnly Date,
    TimeOnly TimeSlot,
    string? Notes,
    List<TreatmentLineItem> Treatments) : IRequest<Guid>;

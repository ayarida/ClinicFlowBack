using MediatR;

namespace ClinicFlow.Application.Features.Clinics.Commands.UpdateClinic;

public record UpdateClinicCommand(
    string Name,
    string Phone,
    string Address,
    string? LogoUrl,
    TimeOnly WorkingHoursStart,
    TimeOnly WorkingHoursEnd,
    int SlotDurationMinutes) : IRequest<Unit>;

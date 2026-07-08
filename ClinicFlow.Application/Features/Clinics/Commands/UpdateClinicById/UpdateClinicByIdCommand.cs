using MediatR;

namespace ClinicFlow.Application.Features.Clinics.Commands.UpdateClinicById;

public record UpdateClinicByIdCommand(
    Guid Id,
    string Name,
    string Phone,
    string Address,
    string? LogoUrl,
    TimeOnly WorkingHoursStart,
    TimeOnly WorkingHoursEnd,
    int SlotDurationMinutes) : IRequest<Unit>;

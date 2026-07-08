using MediatR;

namespace ClinicFlow.Application.Features.Clinics.Commands.CreateClinic;

public record CreateClinicCommand(
    string Name,
    string Phone,
    string Address,
    string? LogoUrl,
    TimeOnly WorkingHoursStart,
    TimeOnly WorkingHoursEnd,
    int SlotDurationMinutes,
    string OwnerName,
    string OwnerEmail) : IRequest<Guid>;

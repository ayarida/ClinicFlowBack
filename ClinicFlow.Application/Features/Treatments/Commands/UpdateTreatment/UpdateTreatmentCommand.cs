using MediatR;

namespace ClinicFlow.Application.Features.Treatments.Commands.UpdateTreatment;

public record UpdateTreatmentCommand(
    Guid Id,
    string Name,
    string? Description,
    int DurationMinutes,
    int? FollowUpIntervalDays,
    decimal? Price
) : IRequest<Unit>;

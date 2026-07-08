using MediatR;

namespace ClinicFlow.Application.Features.Treatments.Commands.CreateTreatment;

public record CreateTreatmentCommand(
    string Name,
    string? Description,
    int DurationMinutes,
    int? FollowUpIntervalDays,
    decimal? Price
) : IRequest<Guid>;

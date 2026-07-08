using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Application.Features.Treatments.DTOs;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Treatments.Queries.GetTreatments;

public class GetTreatmentsQueryHandler(
    ITreatmentRepository treatmentRepository,
    ICurrentClinicService currentClinic
) : IRequestHandler<GetTreatmentsQuery, IReadOnlyList<TreatmentDto>>
{
    public async Task<IReadOnlyList<TreatmentDto>> Handle(GetTreatmentsQuery request, CancellationToken cancellationToken)
    {
        var treatments = await treatmentRepository.GetAllActiveAsync(currentClinic.ClinicId, cancellationToken);

        return treatments
            .Select(t => new TreatmentDto(
                t.Id, t.Name, t.Description, t.DurationMinutes,
                t.FollowUpIntervalDays, t.Price, t.IsActive,
                t.CreatedAt, t.UpdatedAt))
            .ToList();
    }
}

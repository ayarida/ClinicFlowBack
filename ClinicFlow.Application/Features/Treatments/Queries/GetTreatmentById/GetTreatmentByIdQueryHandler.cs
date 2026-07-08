using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Application.Features.Treatments.DTOs;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Treatments.Queries.GetTreatmentById;

public class GetTreatmentByIdQueryHandler(
    ITreatmentRepository treatmentRepository,
    ICurrentClinicService currentClinic
) : IRequestHandler<GetTreatmentByIdQuery, TreatmentDto>
{
    public async Task<TreatmentDto> Handle(GetTreatmentByIdQuery request, CancellationToken cancellationToken)
    {
        var treatment = await treatmentRepository.GetByIdAsync(
            request.Id, currentClinic.ClinicId, cancellationToken);

        if (treatment is null)
            throw new NotFoundException(nameof(Treatment), request.Id);

        return new TreatmentDto(
            treatment.Id, treatment.Name, treatment.Description,
            treatment.DurationMinutes, treatment.FollowUpIntervalDays,
            treatment.Price, treatment.IsActive,
            treatment.CreatedAt, treatment.UpdatedAt);
    }
}

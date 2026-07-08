using ClinicFlow.Application.Features.Clinics.DTOs;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Clinics.Queries.GetClinics;

public class GetClinicsQueryHandler(
    IClinicRepository clinicRepository
) : IRequestHandler<GetClinicsQuery, IReadOnlyList<ClinicDto>>
{
    public async Task<IReadOnlyList<ClinicDto>> Handle(GetClinicsQuery request, CancellationToken cancellationToken)
    {
        var clinics = await clinicRepository.GetAllAsync(cancellationToken);

        return clinics
            .Select(c => new ClinicDto(
                c.Id,
                c.Name,
                c.Phone,
                c.Address,
                c.LogoUrl,
                c.WorkingHoursStart,
                c.WorkingHoursEnd,
                c.SlotDurationMinutes,
                c.IsActive))
            .ToList();
    }
}

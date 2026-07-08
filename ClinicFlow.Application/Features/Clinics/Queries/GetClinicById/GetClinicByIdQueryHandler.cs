using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Features.Clinics.DTOs;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Clinics.Queries.GetClinicById;

public class GetClinicByIdQueryHandler(
    IClinicRepository clinicRepository
) : IRequestHandler<GetClinicByIdQuery, ClinicDto>
{
    public async Task<ClinicDto> Handle(GetClinicByIdQuery request, CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(request.Id, cancellationToken);
        if (clinic is null)
            throw new NotFoundException(nameof(Clinic), request.Id);

        return new ClinicDto(
            clinic.Id,
            clinic.Name,
            clinic.Phone,
            clinic.Address,
            clinic.LogoUrl,
            clinic.WorkingHoursStart,
            clinic.WorkingHoursEnd,
            clinic.SlotDurationMinutes,
            clinic.IsActive);
    }
}

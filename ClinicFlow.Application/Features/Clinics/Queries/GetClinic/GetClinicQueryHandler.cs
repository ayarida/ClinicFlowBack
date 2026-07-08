using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Application.Features.Clinics.DTOs;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Clinics.Queries.GetClinic;

public class GetClinicQueryHandler(
    IClinicRepository clinicRepository,
    ICurrentClinicService currentClinic
) : IRequestHandler<GetClinicQuery, ClinicDto>
{
    public async Task<ClinicDto> Handle(GetClinicQuery request, CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(currentClinic.ClinicId, cancellationToken);
        if (clinic is null)
            throw new NotFoundException(nameof(Clinic), currentClinic.ClinicId);

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

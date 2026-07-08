using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Clinics.Commands.UpdateClinic;

public class UpdateClinicCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork,
    ICurrentClinicService currentClinic
) : IRequestHandler<UpdateClinicCommand, Unit>
{
    public async Task<Unit> Handle(UpdateClinicCommand request, CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(currentClinic.ClinicId, cancellationToken);
        if (clinic is null)
            throw new NotFoundException(nameof(Clinic), currentClinic.ClinicId);

        var nameInUse = await clinicRepository.NameExistsAsync(request.Name, excludeClinicId: currentClinic.ClinicId, cancellationToken);
        if (nameInUse)
            throw new ConflictException($"A clinic named '{request.Name}' already exists.");

        clinic.Update(request.Name, request.Phone, request.Address, request.LogoUrl,
            request.WorkingHoursStart, request.WorkingHoursEnd, request.SlotDurationMinutes);

        clinicRepository.Update(clinic);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

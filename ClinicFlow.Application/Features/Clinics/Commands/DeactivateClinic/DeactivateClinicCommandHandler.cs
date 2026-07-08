using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Clinics.Commands.DeactivateClinic;

public class DeactivateClinicCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeactivateClinicCommand, Unit>
{
    public async Task<Unit> Handle(DeactivateClinicCommand request, CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(request.Id, cancellationToken);
        if (clinic is null)
            throw new NotFoundException(nameof(Clinic), request.Id);

        if (!clinic.IsActive)
            throw new ConflictException($"Clinic '{clinic.Name}' is already inactive.");

        clinic.Deactivate();
        clinicRepository.Update(clinic);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

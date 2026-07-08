using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Clinics.Commands.UpdateClinicById;

public class UpdateClinicByIdCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateClinicByIdCommand, Unit>
{
    public async Task<Unit> Handle(UpdateClinicByIdCommand request, CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(request.Id, cancellationToken);
        if (clinic is null)
            throw new NotFoundException(nameof(Clinic), request.Id);

        var nameInUse = await clinicRepository.NameExistsAsync(request.Name, excludeClinicId: request.Id, cancellationToken);
        if (nameInUse)
            throw new ConflictException($"A clinic named '{request.Name}' already exists.");

        clinic.Update(request.Name, request.Phone, request.Address, request.LogoUrl,
            request.WorkingHoursStart, request.WorkingHoursEnd, request.SlotDurationMinutes);

        clinicRepository.Update(clinic);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

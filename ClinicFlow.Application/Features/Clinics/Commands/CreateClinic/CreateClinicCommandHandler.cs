using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Enums;
using ClinicFlow.Domain.Interfaces;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Clinics.Commands.CreateClinic;

public class CreateClinicCommandHandler(
    IClinicRepository clinicRepository,
    IStaffRepository staffRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateClinicCommand, Guid>
{
    public async Task<Guid> Handle(CreateClinicCommand request, CancellationToken cancellationToken)
    {
        var nameInUse = await clinicRepository.NameExistsAsync(request.Name, excludeClinicId: null, cancellationToken);
        if (nameInUse)
            throw new ConflictException($"A clinic named '{request.Name}' already exists.");

        var ownerEmailInUse = await staffRepository.GetByEmailAsync(request.OwnerEmail, cancellationToken);
        if (ownerEmailInUse is not null)
            throw new ConflictException($"A staff member with email '{request.OwnerEmail}' already exists.");

        var clinic = Clinic.Create(
            request.Name,
            request.Phone,
            request.Address,
            request.LogoUrl,
            request.WorkingHoursStart,
            request.WorkingHoursEnd,
            request.SlotDurationMinutes);

        await clinicRepository.AddAsync(clinic, cancellationToken);

        var owner = global::ClinicFlow.Domain.Entities.Staff.Create(clinic.Id, request.OwnerName, request.OwnerEmail, StaffRole.Owner);
        await staffRepository.AddAsync(owner, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return clinic.Id;
    }
}

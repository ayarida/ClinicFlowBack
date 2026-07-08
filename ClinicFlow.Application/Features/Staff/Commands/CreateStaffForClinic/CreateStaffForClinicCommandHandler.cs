using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Staff.Commands.CreateStaffForClinic;

public class CreateStaffForClinicCommandHandler(
    IClinicRepository clinicRepository,
    IStaffRepository staffRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateStaffForClinicCommand, Guid>
{
    public async Task<Guid> Handle(CreateStaffForClinicCommand request, CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken);
        if (clinic is null)
            throw new NotFoundException(nameof(Clinic), request.ClinicId);

        var emailInUse = await staffRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (emailInUse is not null)
            throw new ConflictException($"A staff member with email '{request.Email}' already exists.");

        var staff = global::ClinicFlow.Domain.Entities.Staff.Create(request.ClinicId, request.Name, request.Email, request.Role);

        await staffRepository.AddAsync(staff, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return staff.Id;
    }
}

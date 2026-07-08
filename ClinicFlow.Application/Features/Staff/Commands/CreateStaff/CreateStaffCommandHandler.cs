using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Domain.Interfaces;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Staff.Commands.CreateStaff;

public class CreateStaffCommandHandler(
    IStaffRepository staffRepository,
    IUnitOfWork unitOfWork,
    ICurrentClinicService currentClinic
) : IRequestHandler<CreateStaffCommand, Guid>
{
    public async Task<Guid> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
    {
        var emailInUse = await staffRepository.EmailExistsAsync(
            currentClinic.ClinicId, request.Email, excludeStaffId: null, cancellationToken);
        if (emailInUse)
            throw new ConflictException($"A staff member with email '{request.Email}' already exists.");

        var staff = global::ClinicFlow.Domain.Entities.Staff.Create(currentClinic.ClinicId, request.Name, request.Email, request.Role);

        await staffRepository.AddAsync(staff, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return staff.Id;
    }
}

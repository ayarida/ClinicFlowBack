using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Domain.Interfaces;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Staff.Commands.DeactivateStaff;

public class DeactivateStaffCommandHandler(
    IStaffRepository staffRepository,
    IUnitOfWork unitOfWork,
    ICurrentClinicService currentClinic
) : IRequestHandler<DeactivateStaffCommand, Unit>
{
    public async Task<Unit> Handle(DeactivateStaffCommand request, CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetByIdAsync(request.Id, currentClinic.ClinicId, cancellationToken);
        if (staff is null)
            throw new NotFoundException(nameof(Staff), request.Id);

        if (!staff.IsActive)
            throw new ConflictException($"Staff member '{staff.Name}' is already inactive.");

        staff.Deactivate();
        staffRepository.Update(staff);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

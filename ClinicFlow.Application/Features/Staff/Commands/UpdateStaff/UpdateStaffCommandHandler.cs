using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Domain.Interfaces;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Staff.Commands.UpdateStaff;

public class UpdateStaffCommandHandler(
    IStaffRepository staffRepository,
    IUnitOfWork unitOfWork,
    ICurrentClinicService currentClinic
) : IRequestHandler<UpdateStaffCommand, Unit>
{
    public async Task<Unit> Handle(UpdateStaffCommand request, CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetByIdAsync(request.Id, currentClinic.ClinicId, cancellationToken);
        if (staff is null)
            throw new NotFoundException(nameof(Staff), request.Id);

        staff.Update(request.Name, request.Role);
        staffRepository.Update(staff);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

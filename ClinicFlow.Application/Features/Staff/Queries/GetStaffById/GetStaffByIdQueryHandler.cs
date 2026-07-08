using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Application.Features.Staff.DTOs;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Staff.Queries.GetStaffById;

public class GetStaffByIdQueryHandler(
    IStaffRepository staffRepository,
    ICurrentClinicService currentClinic
) : IRequestHandler<GetStaffByIdQuery, StaffDto>
{
    public async Task<StaffDto> Handle(GetStaffByIdQuery request, CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetByIdAsync(request.Id, currentClinic.ClinicId, cancellationToken);
        if (staff is null)
            throw new NotFoundException(nameof(Staff), request.Id);

        return new StaffDto(staff.Id, staff.ClinicId, staff.Name, staff.Email, staff.Role, staff.IsActive);
    }
}

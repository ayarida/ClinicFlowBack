using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Application.Features.Staff.DTOs;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Staff.Queries.GetStaff;

public class GetStaffQueryHandler(
    IStaffRepository staffRepository,
    ICurrentClinicService currentClinic
) : IRequestHandler<GetStaffQuery, IReadOnlyList<StaffDto>>
{
    public async Task<IReadOnlyList<StaffDto>> Handle(GetStaffQuery request, CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetAllAsync(currentClinic.ClinicId, cancellationToken);
        return staff.Select(s => new StaffDto(s.Id, s.ClinicId, s.Name, s.Email, s.Role, s.IsActive)).ToList();
    }
}

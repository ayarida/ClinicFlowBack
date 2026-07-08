using ClinicFlow.Domain.Enums;
using MediatR;

namespace ClinicFlow.Application.Features.Staff.Commands.UpdateStaff;

public record UpdateStaffCommand(
    Guid Id,
    string Name,
    StaffRole Role) : IRequest<Unit>;

using ClinicFlow.Domain.Enums;
using MediatR;

namespace ClinicFlow.Application.Features.Staff.Commands.CreateStaff;

public record CreateStaffCommand(
    string Name,
    string Email,
    StaffRole Role) : IRequest<Guid>;

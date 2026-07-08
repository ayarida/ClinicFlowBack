using ClinicFlow.Domain.Enums;
using MediatR;

namespace ClinicFlow.Application.Features.Staff.Commands.CreateStaffForClinic;

public record CreateStaffForClinicCommand(
    Guid ClinicId,
    string Name,
    string Email,
    StaffRole Role) : IRequest<Guid>;

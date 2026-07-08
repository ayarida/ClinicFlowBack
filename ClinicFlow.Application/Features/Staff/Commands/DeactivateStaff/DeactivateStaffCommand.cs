using MediatR;

namespace ClinicFlow.Application.Features.Staff.Commands.DeactivateStaff;

public record DeactivateStaffCommand(Guid Id) : IRequest<Unit>;

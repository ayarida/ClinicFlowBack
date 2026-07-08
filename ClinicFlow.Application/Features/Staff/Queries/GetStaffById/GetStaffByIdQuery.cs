using ClinicFlow.Application.Features.Staff.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Staff.Queries.GetStaffById;

public record GetStaffByIdQuery(Guid Id) : IRequest<StaffDto>;

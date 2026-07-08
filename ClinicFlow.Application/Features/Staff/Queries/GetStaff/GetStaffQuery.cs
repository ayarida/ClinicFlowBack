using ClinicFlow.Application.Features.Staff.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Staff.Queries.GetStaff;

public record GetStaffQuery : IRequest<IReadOnlyList<StaffDto>>;

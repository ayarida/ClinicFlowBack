using ClinicFlow.Application.Features.Clinics.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Clinics.Queries.GetClinicById;

public record GetClinicByIdQuery(Guid Id) : IRequest<ClinicDto>;

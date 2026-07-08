using ClinicFlow.Application.Features.Clinics.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Clinics.Queries.GetClinics;

public record GetClinicsQuery : IRequest<IReadOnlyList<ClinicDto>>;

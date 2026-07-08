using ClinicFlow.Application.Features.Clinics.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Clinics.Queries.GetClinic;

public record GetClinicQuery : IRequest<ClinicDto>;

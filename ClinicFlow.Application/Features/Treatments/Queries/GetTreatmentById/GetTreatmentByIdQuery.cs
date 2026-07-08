using ClinicFlow.Application.Features.Treatments.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Treatments.Queries.GetTreatmentById;

public record GetTreatmentByIdQuery(Guid Id) : IRequest<TreatmentDto>;

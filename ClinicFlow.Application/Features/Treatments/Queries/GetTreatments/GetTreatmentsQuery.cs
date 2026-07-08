using ClinicFlow.Application.Features.Treatments.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Treatments.Queries.GetTreatments;

public record GetTreatmentsQuery : IRequest<IReadOnlyList<TreatmentDto>>;

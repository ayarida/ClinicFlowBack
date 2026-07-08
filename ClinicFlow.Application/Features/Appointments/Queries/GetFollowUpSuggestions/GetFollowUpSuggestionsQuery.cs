using ClinicFlow.Application.Features.Appointments.DTOs;
using MediatR;

namespace ClinicFlow.Application.Features.Appointments.Queries.GetFollowUpSuggestions;

public record GetFollowUpSuggestionsQuery(Guid AppointmentId)
    : IRequest<IReadOnlyList<FollowUpSuggestionDto>>;

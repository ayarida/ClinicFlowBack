using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Application.Features.Appointments.DTOs;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Enums;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Appointments.Queries.GetFollowUpSuggestions;

public class GetFollowUpSuggestionsQueryHandler(
    IAppointmentRepository appointmentRepository,
    ICurrentClinicService currentClinic
) : IRequestHandler<GetFollowUpSuggestionsQuery, IReadOnlyList<FollowUpSuggestionDto>>
{
    public async Task<IReadOnlyList<FollowUpSuggestionDto>> Handle(
        GetFollowUpSuggestionsQuery request,
        CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetByIdAsync(
            request.AppointmentId, currentClinic.ClinicId, cancellationToken);

        if (appointment is null)
            throw new NotFoundException(nameof(Appointment), request.AppointmentId);

        if (appointment.Status != AppointmentStatus.Completed)
            throw new ConflictException("Follow-up suggestions are only available for completed appointments.");

        return appointment.Treatments
            .Where(at => at.Treatment.FollowUpIntervalDays.HasValue)
            .Select(at => new FollowUpSuggestionDto(
                at.TreatmentId,
                at.Treatment.Name,
                at.Treatment.FollowUpIntervalDays!.Value,
                appointment.Date.AddDays(at.Treatment.FollowUpIntervalDays!.Value)))
            .ToList();
    }
}

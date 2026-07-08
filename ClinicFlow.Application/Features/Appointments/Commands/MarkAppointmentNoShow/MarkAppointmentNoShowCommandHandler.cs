using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Enums;
using ClinicFlow.Domain.Interfaces;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Appointments.Commands.MarkAppointmentNoShow;

public class MarkAppointmentNoShowCommandHandler(
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork,
    ICurrentClinicService currentClinic
) : IRequestHandler<MarkAppointmentNoShowCommand, Unit>
{
    public async Task<Unit> Handle(MarkAppointmentNoShowCommand request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetByIdAsync(request.Id, currentClinic.ClinicId, cancellationToken);
        if (appointment is null)
            throw new NotFoundException(nameof(Appointment), request.Id);

        if (appointment.Status != AppointmentStatus.Scheduled)
            throw new ConflictException("Only scheduled appointments can be marked as no-show.");

        appointment.MarkNoShow();
        appointmentRepository.Update(appointment);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

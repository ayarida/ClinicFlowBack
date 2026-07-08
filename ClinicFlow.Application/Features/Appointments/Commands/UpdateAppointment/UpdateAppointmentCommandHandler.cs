using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Enums;
using ClinicFlow.Domain.Interfaces;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Appointments.Commands.UpdateAppointment;

public class UpdateAppointmentCommandHandler(
    IAppointmentRepository appointmentRepository,
    ITreatmentRepository treatmentRepository,
    IUnitOfWork unitOfWork,
    ICurrentClinicService currentClinic
) : IRequestHandler<UpdateAppointmentCommand, Unit>
{
    public async Task<Unit> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetByIdAsync(request.Id, currentClinic.ClinicId, cancellationToken);
        if (appointment is null)
            throw new NotFoundException(nameof(Appointment), request.Id);

        if (appointment.Status != AppointmentStatus.Scheduled)
            throw new ConflictException("Only scheduled appointments can be updated.");

        var slotTaken = await appointmentRepository.TimeSlotTakenAsync(
            currentClinic.ClinicId, request.Date, request.TimeSlot, excludeAppointmentId: request.Id, cancellationToken);
        if (slotTaken)
            throw new ConflictException($"The time slot {request.TimeSlot} on {request.Date} is already booked.");

        var newTreatments = new List<AppointmentTreatment>();
        foreach (var item in request.Treatments)
        {
            var treatment = await treatmentRepository.GetByIdAsync(item.TreatmentId, currentClinic.ClinicId, cancellationToken);
            if (treatment is null || !treatment.IsActive)
                throw new NotFoundException(nameof(Treatment), item.TreatmentId);

            newTreatments.Add(AppointmentTreatment.Create(appointment.Id, item.TreatmentId, item.Notes));
        }

        appointment.Update(request.Date, request.TimeSlot, request.Notes);
        appointmentRepository.ReplaceTreatments(appointment, newTreatments);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

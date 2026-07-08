using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandHandler(
    IAppointmentRepository appointmentRepository,
    ICustomerRepository customerRepository,
    ITreatmentRepository treatmentRepository,
    IUnitOfWork unitOfWork,
    ICurrentClinicService currentClinic
) : IRequestHandler<CreateAppointmentCommand, Guid>
{
    public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(request.CustomerId, currentClinic.ClinicId, cancellationToken);
        if (customer is null)
            throw new NotFoundException(nameof(Customer), request.CustomerId);

        var slotTaken = await appointmentRepository.TimeSlotTakenAsync(
            currentClinic.ClinicId, request.Date, request.TimeSlot, excludeAppointmentId: null, cancellationToken);
        if (slotTaken)
            throw new ConflictException($"The time slot {request.TimeSlot} on {request.Date} is already booked.");

        var appointment = Appointment.Create(
            currentClinic.ClinicId,
            request.CustomerId,
            request.Date,
            request.TimeSlot,
            request.Notes);

        foreach (var item in request.Treatments)
        {
            var treatment = await treatmentRepository.GetByIdAsync(item.TreatmentId, currentClinic.ClinicId, cancellationToken);
            if (treatment is null || !treatment.IsActive)
                throw new NotFoundException(nameof(Treatment), item.TreatmentId);

            appointment.Treatments.Add(AppointmentTreatment.Create(appointment.Id, item.TreatmentId, item.Notes));
        }

        await appointmentRepository.AddAsync(appointment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return appointment.Id;
    }
}

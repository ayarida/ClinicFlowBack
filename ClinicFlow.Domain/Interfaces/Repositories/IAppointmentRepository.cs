using ClinicFlow.Domain.Entities;

namespace ClinicFlow.Domain.Interfaces.Repositories;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(Guid id, Guid clinicId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Appointment>> GetAllAsync(Guid clinicId, DateOnly? date, Guid? customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Appointment>> GetByMonthAsync(Guid clinicId, int year, int month, CancellationToken cancellationToken = default);
    Task AddAsync(Appointment appointment, CancellationToken cancellationToken = default);
    void Update(Appointment appointment);
    void ReplaceTreatments(Appointment appointment, IEnumerable<AppointmentTreatment> newTreatments);
    Task<bool> TimeSlotTakenAsync(Guid clinicId, DateOnly date, TimeOnly timeSlot, Guid? excludeAppointmentId, CancellationToken cancellationToken = default);
}

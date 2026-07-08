using ClinicFlow.Domain.Enums;

namespace ClinicFlow.Domain.Entities;

public class Appointment : BaseEntity
{
    public Guid ClinicId { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateOnly Date { get; private set; }
    public TimeOnly TimeSlot { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public string? Notes { get; private set; }

    public Customer Customer { get; private set; } = default!;
    public List<AppointmentTreatment> Treatments { get; private set; } = [];

    private Appointment() { }

    public static Appointment Create(
        Guid clinicId,
        Guid customerId,
        DateOnly date,
        TimeOnly timeSlot,
        string? notes)
    {
        return new Appointment
        {
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            CustomerId = customerId,
            Date = date,
            TimeSlot = timeSlot,
            Status = AppointmentStatus.Scheduled,
            Notes = notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Update(DateOnly date, TimeOnly timeSlot, string? notes)
    {
        Date = date;
        TimeSlot = timeSlot;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = AppointmentStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        Status = AppointmentStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkNoShow()
    {
        Status = AppointmentStatus.NoShow;
        UpdatedAt = DateTime.UtcNow;
    }
}

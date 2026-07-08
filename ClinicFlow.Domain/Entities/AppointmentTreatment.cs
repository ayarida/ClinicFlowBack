namespace ClinicFlow.Domain.Entities;

public class AppointmentTreatment : BaseEntity
{
    public Guid AppointmentId { get; private set; }
    public Guid TreatmentId { get; private set; }
    public string? Notes { get; private set; }

    public Treatment Treatment { get; private set; } = default!;

    private AppointmentTreatment() { }

    public static AppointmentTreatment Create(Guid appointmentId, Guid treatmentId, string? notes)
    {
        return new AppointmentTreatment
        {
            Id = Guid.NewGuid(),
            AppointmentId = appointmentId,
            TreatmentId = treatmentId,
            Notes = notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}

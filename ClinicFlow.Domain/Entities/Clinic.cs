namespace ClinicFlow.Domain.Entities;

public class Clinic : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string Phone { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public string? LogoUrl { get; private set; }
    public TimeOnly WorkingHoursStart { get; private set; }
    public TimeOnly WorkingHoursEnd { get; private set; }
    public int SlotDurationMinutes { get; private set; }
    public bool IsActive { get; private set; }

    private Clinic() { }

    public static Clinic Create(
        string name,
        string phone,
        string address,
        string? logoUrl,
        TimeOnly workingHoursStart,
        TimeOnly workingHoursEnd,
        int slotDurationMinutes)
    {
        return new Clinic
        {
            Id = Guid.NewGuid(),
            Name = name,
            Phone = phone,
            Address = address,
            LogoUrl = logoUrl,
            WorkingHoursStart = workingHoursStart,
            WorkingHoursEnd = workingHoursEnd,
            SlotDurationMinutes = slotDurationMinutes,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Update(
        string name,
        string phone,
        string address,
        string? logoUrl,
        TimeOnly workingHoursStart,
        TimeOnly workingHoursEnd,
        int slotDurationMinutes)
    {
        Name = name;
        Phone = phone;
        Address = address;
        LogoUrl = logoUrl;
        WorkingHoursStart = workingHoursStart;
        WorkingHoursEnd = workingHoursEnd;
        SlotDurationMinutes = slotDurationMinutes;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}

namespace ClinicFlow.Domain.Entities;

public class Treatment : BaseEntity
{
    public Guid ClinicId { get; private set; }
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public int DurationMinutes { get; private set; }
    public int? FollowUpIntervalDays { get; private set; }
    public decimal? Price { get; private set; }
    public bool IsActive { get; private set; }

    private Treatment() { }

    public static Treatment Create(
        Guid clinicId,
        string name,
        string? description,
        int durationMinutes,
        int? followUpIntervalDays,
        decimal? price)
    {
        return new Treatment
        {
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            Name = name,
            Description = description,
            DurationMinutes = durationMinutes,
            FollowUpIntervalDays = followUpIntervalDays,
            Price = price,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Update(
        string name,
        string? description,
        int durationMinutes,
        int? followUpIntervalDays,
        decimal? price)
    {
        Name = name;
        Description = description;
        DurationMinutes = durationMinutes;
        FollowUpIntervalDays = followUpIntervalDays;
        Price = price;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}

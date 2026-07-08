using ClinicFlow.Domain.Enums;

namespace ClinicFlow.Domain.Entities;

public class Staff : BaseEntity
{
    public Guid ClinicId { get; private set; }
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public StaffRole Role { get; private set; }
    public bool IsActive { get; private set; }

    private Staff() { }

    public static Staff Create(
        Guid clinicId,
        string name,
        string email,
        StaffRole role)
    {
        return new Staff
        {
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            Name = name,
            Email = email,
            Role = role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Update(string name, StaffRole role)
    {
        Name = name;
        Role = role;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}

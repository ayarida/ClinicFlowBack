using ClinicFlow.Domain.Enums;

namespace ClinicFlow.Domain.Entities;

public class Customer : BaseEntity
{
    public Guid ClinicId { get; private set; }
    public string Name { get; private set; } = default!;
    public string Phone { get; private set; } = default!;
    public string? Email { get; private set; }
    public DateOnly? DateOfBirth { get; private set; }
    public Gender? Gender { get; private set; }
    public string? Notes { get; private set; }

    private Customer() { }

    public static Customer Create(
        Guid clinicId,
        string name,
        string phone,
        string? email,
        DateOnly? dateOfBirth,
        Gender? gender,
        string? notes)
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            Name = name,
            Phone = phone,
            Email = email,
            DateOfBirth = dateOfBirth,
            Gender = gender,
            Notes = notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Update(
        string name,
        string phone,
        string? email,
        DateOnly? dateOfBirth,
        Gender? gender,
        string? notes)
    {
        Name = name;
        Phone = phone;
        Email = email;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }
}

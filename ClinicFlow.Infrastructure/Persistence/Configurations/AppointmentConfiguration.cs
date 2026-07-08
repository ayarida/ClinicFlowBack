using ClinicFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicFlow.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.ClinicId).IsRequired();
        builder.Property(a => a.CustomerId).IsRequired();
        builder.Property(a => a.Date).IsRequired();
        builder.Property(a => a.TimeSlot).IsRequired();
        builder.Property(a => a.Status).IsRequired();
        builder.Property(a => a.Notes).HasMaxLength(2000);
        builder.Property(a => a.CreatedAt).IsRequired();
        builder.Property(a => a.UpdatedAt).IsRequired();

        builder.HasOne<Clinic>()
            .WithMany()
            .HasForeignKey(a => a.ClinicId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Customer)
            .WithMany()
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.Treatments)
            .WithOne()
            .HasForeignKey(at => at.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => new { a.ClinicId, a.Date })
            .HasDatabaseName("IX_Appointments_ClinicId_Date");

        builder.HasIndex(a => new { a.ClinicId, a.CustomerId })
            .HasDatabaseName("IX_Appointments_ClinicId_CustomerId");
    }
}

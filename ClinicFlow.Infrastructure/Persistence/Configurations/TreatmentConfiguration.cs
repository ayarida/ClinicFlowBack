using ClinicFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicFlow.Infrastructure.Persistence.Configurations;

public class TreatmentConfiguration : IEntityTypeConfiguration<Treatment>
{
    public void Configure(EntityTypeBuilder<Treatment> builder)
    {
        builder.ToTable("Treatments");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.ClinicId)
            .IsRequired();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        builder.Property(t => t.DurationMinutes)
            .IsRequired();

        builder.Property(t => t.FollowUpIntervalDays);

        builder.Property(t => t.Price)
            .HasColumnType("decimal(10,2)");

        builder.Property(t => t.IsActive)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .IsRequired();

        builder.HasOne<Clinic>()
            .WithMany()
            .HasForeignKey(t => t.ClinicId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => new { t.ClinicId, t.IsActive })
            .HasDatabaseName("IX_Treatments_ClinicId_IsActive");

        builder.HasIndex(t => new { t.ClinicId, t.Name })
            .HasDatabaseName("IX_Treatments_ClinicId_Name");
    }
}

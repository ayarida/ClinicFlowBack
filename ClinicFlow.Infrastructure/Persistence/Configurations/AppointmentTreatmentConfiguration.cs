using ClinicFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicFlow.Infrastructure.Persistence.Configurations;

public class AppointmentTreatmentConfiguration : IEntityTypeConfiguration<AppointmentTreatment>
{
    public void Configure(EntityTypeBuilder<AppointmentTreatment> builder)
    {
        builder.ToTable("AppointmentTreatments");

        builder.HasKey(at => at.Id);

        builder.Property(at => at.AppointmentId).IsRequired();
        builder.Property(at => at.TreatmentId).IsRequired();
        builder.Property(at => at.Notes).HasMaxLength(1000);
        builder.Property(at => at.CreatedAt).IsRequired();
        builder.Property(at => at.UpdatedAt).IsRequired();

        builder.HasOne(at => at.Treatment)
            .WithMany()
            .HasForeignKey(at => at.TreatmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(at => at.AppointmentId)
            .HasDatabaseName("IX_AppointmentTreatments_AppointmentId");
    }
}

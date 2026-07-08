using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicFlow.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.ClinicId)
            .IsRequired();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Phone)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Email)
            .HasMaxLength(300);

        builder.Property(c => c.Gender)
            .HasConversion<int?>()
            .IsRequired(false);

        builder.Property(c => c.Notes)
            .HasColumnType("nvarchar(max)");

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        builder.HasOne<Clinic>()
            .WithMany()
            .HasForeignKey(c => c.ClinicId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => new { c.ClinicId, c.Name })
            .HasDatabaseName("IX_Customers_ClinicId_Name");

        builder.HasIndex(c => new { c.ClinicId, c.Phone })
            .HasDatabaseName("IX_Customers_ClinicId_Phone");
    }
}

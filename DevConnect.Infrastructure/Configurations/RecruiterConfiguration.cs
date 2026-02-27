using DevConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevConnect.Infrastructure.Configurations;

public class RecruiterConfiguration : IEntityTypeConfiguration<Recruiter>
{
    public void Configure(EntityTypeBuilder<Recruiter> builder)
    {
        builder.ToTable("Recruiters");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(200);

        // Unique index for Email
        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        // Unique index for PhoneNumber
        builder.HasIndex(x => x.PhoneNumber)
            .IsUnique();

        builder.Property(x => x.CompanyAddress)
            .HasMaxLength(300);

        builder.Property(x => x.CompanyRole)
            .HasMaxLength(100);

        builder.Property(x => x.State)
            .HasConversion<int>()
            .IsRequired();

        // Check Constraints for Email and PhoneNumber (PostgreSQL regex checks)
        builder.HasCheckConstraint("CK_Recruiters_Email", "\"Email\" ~ '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$'");
        builder.HasCheckConstraint("CK_Recruiters_PhoneNumber", "\"PhoneNumber\" ~ '^\\+?[0-9]{7,15}$'");
    }
}

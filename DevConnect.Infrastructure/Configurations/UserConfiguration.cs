using DevConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevConnect.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.PasswordHash)
            .IsRequired();

        builder.Property(x => x.ImageUrl)
            .HasMaxLength(500);

        builder.Property(x => x.PortfolioUrl)
            .HasMaxLength(500);

        // Enum → int
        builder.Property(x => x.Role)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Profession)
            .HasConversion<int>();

        builder.Property(x => x.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.State)
            .HasConversion<int>()
            .IsRequired();

        // 1-1 relation with UserToken
        builder.HasOne<UserToken>()
            .WithOne()
            .HasForeignKey<UserToken>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // 1-1 relation with Recruiter
        builder.HasOne(x => x.Recruiter)
            .WithOne(x => x.User)
            .HasForeignKey<Recruiter>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
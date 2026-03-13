using DevConnect.Domain.Entities;
using DevConnect.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevConnect.Infrastructure.Configurations;

public class InterviewConfiguration : IEntityTypeConfiguration<Interview>
{
    public void Configure(EntityTypeBuilder<Interview> builder)
    {
        builder.ToTable("Interviews");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.InterviewDate)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.Property(x => x.MeetingLink)
            .HasMaxLength(500);

        builder.Property(x => x.State)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Score);

        builder.Property(x => x.ResultNote)
            .HasMaxLength(2000);

        // FK: UserId → Users (suhbatga chaqirilgan user)
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // FK: LogCheckerId → Users (suhbat o'tkazuvchi LogChecker)
        builder.HasOne(x => x.LogChecker)
            .WithMany()
            .HasForeignKey(x => x.LogCheckerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indekslar — tezkor qidirish uchun
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.LogCheckerId);
        builder.HasIndex(x => x.InterviewDate);
    }
}

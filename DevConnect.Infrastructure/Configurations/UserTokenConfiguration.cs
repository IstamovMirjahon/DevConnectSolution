using DevConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevConnect.Infrastructure.Configurations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable("UserTokens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AccessToken)
            .IsRequired();

        builder.Property(x => x.RefreshToken)
            .IsRequired();

        builder.HasIndex(x => x.RefreshToken)
            .IsUnique();

        builder.Property(x => x.AccessTokenExpiration)
            .IsRequired();

        builder.Property(x => x.RefreshTokenExpiration)
            .IsRequired();

        builder.Property(x => x.PasswordToken);

        builder.Property(x => x.PasswordTokenExpiration);

        builder.Property(x => x.UserId)
            .IsRequired();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder
            .Property(x => x.JwtId)
            .HasVarcharMaxLength(40)
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.UserSession)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserSessionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.UserId);

        builder
            .HasIndex(x => x.JwtId);
    }
}
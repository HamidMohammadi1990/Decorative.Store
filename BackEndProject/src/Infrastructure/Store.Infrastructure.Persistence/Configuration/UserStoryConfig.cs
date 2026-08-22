using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class UserStoryConfig : IEntityTypeConfiguration<UserStory>
{
    public void Configure(EntityTypeBuilder<UserStory> builder)
    {
        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(70)
            .IsRequired();

        builder
            .Property(x => x.Caption)
            .HasNVarcharMaxLength(500);

        builder
            .Property(x => x.MediaPath)
            .HasNVarcharMaxLength(260)
            .IsRequired();

        builder
            .Property(x => x.MediaAlt)
            .HasNVarcharMaxLength(120)
            .IsRequired();

        builder
            .Property(x => x.PosterPath)
            .HasNVarcharMaxLength(260);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.UserStories)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.UserStories)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasIndex(x => x.UserId);

        builder
            .HasIndex(x => new { x.IsActive, x.CreatedOnUtc });
    }
}

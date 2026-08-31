using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

public class UserStoryLikeConfig : IEntityTypeConfiguration<UserStoryLike>
{
    public void Configure(EntityTypeBuilder<UserStoryLike> builder)
    {
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.UserStoryLikes)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.UserStory)
            .WithMany(x => x.Likes)
            .HasForeignKey(x => x.UserStoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.UserStoryId);

        builder
            .HasIndex(x => new { x.UserId, x.UserStoryId })
            .IsUnique();
    }
}

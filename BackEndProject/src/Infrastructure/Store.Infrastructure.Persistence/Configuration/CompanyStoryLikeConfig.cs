using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

public class CompanyStoryLikeConfig : IEntityTypeConfiguration<CompanyStoryLike>
{
    public void Configure(EntityTypeBuilder<CompanyStoryLike> builder)
    {
        builder
            .Property(x => x.ClientIP)
            .HasMaxLength(15);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.CompanyStoryLikes)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.CompanyStory)
            .WithMany(x => x.Likes)
            .HasForeignKey(x => x.CompanyStoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.UserId);

        builder
            .HasIndex(x => x.CompanyStoryId);
    }
}

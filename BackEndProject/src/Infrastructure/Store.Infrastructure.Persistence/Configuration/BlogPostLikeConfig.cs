using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

public class BlogPostLikeConfig : IEntityTypeConfiguration<BlogPostLike>
{
    public void Configure(EntityTypeBuilder<BlogPostLike> builder)
    {
        builder
            .Property(x => x.ClientIP)
            .HasMaxLength(15);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.BlogPostLikes)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.BlogPost)
            .WithMany(x => x.BlogPostLikes)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.UserId);

        builder
            .HasIndex(x => x.BlogPostId);
    }
}
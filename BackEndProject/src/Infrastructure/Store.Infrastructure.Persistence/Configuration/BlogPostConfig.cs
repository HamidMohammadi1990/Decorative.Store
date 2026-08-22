using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class BlogPostConfig : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder
            .Property(x => x.Code)
            .HasVarcharMaxLength(30)
            .IsRequired();

        builder
            .HasIndex(x => x.Code)
            .IsUnique();

        builder
            .HasOne(x => x.BlogPostCategory)
            .WithMany(x => x.BlogPosts)
            .HasForeignKey(x => x.BlogPostCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.BlogPosts)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.BlogPostTags)
            .WithOne(x => x.BlogPost)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.BlogPostLikes)
            .WithOne(x => x.BlogPost)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.BlogPostComments)
            .WithOne(x => x.BlogPost)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.BlogPostCategoryId);

        builder
            .HasIndex(x => x.UserId);
    }
}

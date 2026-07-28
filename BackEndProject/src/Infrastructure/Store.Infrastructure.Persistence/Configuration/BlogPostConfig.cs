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
            .Property(x => x.Title)
            .HasNVarcharMaxLength(70);

        builder
            .Property(x => x.Slug)
            .HasVarcharMaxLength(150);

        builder
            .Property(x => x.MetaDescription)
            .HasNVarcharMaxLength(200);

        builder
            .Property(x => x.SeoKeywords)
            .HasNVarcharMaxLength(150);

        builder
            .Property(x => x.Content)
            .HasNVarcharMaxLength(2500);

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

        builder
            .HasIndex(x => x.Slug)
            .IsUnique();
    }
}
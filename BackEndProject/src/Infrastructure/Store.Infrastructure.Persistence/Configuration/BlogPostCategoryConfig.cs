using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class BlogPostCategoryConfig : IEntityTypeConfiguration<BlogPostCategory>
{
    public void Configure(EntityTypeBuilder<BlogPostCategory> builder)
    {
        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(70);

        builder
            .Property(x => x.Slug)
            .HasVarcharMaxLength(150);

        builder
            .HasMany(x => x.BlogPosts)
            .WithOne(x => x.BlogPostCategory)
            .HasForeignKey(x => x.BlogPostCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.Slug)
            .IsUnique();
    }
}
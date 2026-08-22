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
            .Property(x => x.Code)
            .HasVarcharMaxLength(30)
            .IsRequired();

        builder
            .HasIndex(x => x.Code)
            .IsUnique();

        builder
            .HasMany(x => x.BlogPosts)
            .WithOne(x => x.BlogPostCategory)
            .HasForeignKey(x => x.BlogPostCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

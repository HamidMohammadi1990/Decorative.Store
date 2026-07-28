using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

public class BlogPostTagConfig : IEntityTypeConfiguration<BlogPostTag>
{
    public void Configure(EntityTypeBuilder<BlogPostTag> builder)
    {
        builder
            .HasOne(x => x.Tag)
            .WithMany(x => x.BlogPostTags)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.BlogPost)
            .WithMany(x => x.BlogPostTags)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.BlogPostId);

        builder
            .HasIndex(x => x.TagId);
    }
}
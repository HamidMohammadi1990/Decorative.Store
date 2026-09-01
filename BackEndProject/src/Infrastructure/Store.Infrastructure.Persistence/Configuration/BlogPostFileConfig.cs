using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class BlogPostFileConfig : IEntityTypeConfiguration<BlogPostFile>
{
    public void Configure(EntityTypeBuilder<BlogPostFile> builder)
    {
        builder
            .Property(x => x.FileName)
            .HasVarcharMaxLength(70)
            .IsRequired();

        builder
            .HasOne(d => d.BlogPost)
            .WithMany(p => p.BlogPostFiles)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.BlogPostId);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class BlogPostFileTranslationConfig : IEntityTypeConfiguration<BlogPostFileTranslation>
{
    public void Configure(EntityTypeBuilder<BlogPostFileTranslation> builder)
    {
        builder.ToTable("BlogPostFileTranslation");

        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(30)
            .IsRequired();

        builder
            .HasOne(x => x.BlogPostFile)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.BlogPostFileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.BlogPostFileId, x.LanguageId })
            .IsUnique();
    }
}

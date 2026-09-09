using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class BlogPostTranslationConfig : IEntityTypeConfiguration<BlogPostTranslation>
{
    public void Configure(EntityTypeBuilder<BlogPostTranslation> builder)
    {
        builder.ToTable("BlogPostTranslation");

        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(70)
            .IsRequired();

        builder
            .Property(x => x.Slug)
            .HasNVarcharMaxLength(150)
            .IsRequired();

        builder
            .Property(x => x.MetaDescription)
            .HasNVarcharMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.SeoKeywords)
            .HasNVarcharMaxLength(150)
            .IsRequired();

        builder
            .Property(x => x.Content)
            .HasNVarcharMaxLength(2500)
            .IsRequired();

        builder
            .HasOne(x => x.BlogPost)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.BlogPostId, x.LanguageId })
            .IsUnique();

        builder
            .HasIndex(x => new { x.LanguageId, x.Slug })
            .IsUnique();
    }
}

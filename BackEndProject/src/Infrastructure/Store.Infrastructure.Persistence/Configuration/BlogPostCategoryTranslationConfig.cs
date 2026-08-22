using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class BlogPostCategoryTranslationConfig : IEntityTypeConfiguration<BlogPostCategoryTranslation>
{
    public void Configure(EntityTypeBuilder<BlogPostCategoryTranslation> builder)
    {
        builder.ToTable("BlogPostCategoryTranslation");

        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(70)
            .IsRequired();

        builder
            .Property(x => x.Slug)
            .HasVarcharMaxLength(150)
            .IsRequired();

        builder
            .HasOne(x => x.BlogPostCategory)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.BlogPostCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.BlogPostCategoryId, x.LanguageId })
            .IsUnique();

        builder
            .HasIndex(x => new { x.LanguageId, x.Slug })
            .IsUnique();
    }
}

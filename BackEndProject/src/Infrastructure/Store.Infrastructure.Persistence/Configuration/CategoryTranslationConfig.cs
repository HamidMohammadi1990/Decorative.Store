using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class CategoryTranslationConfig : IEntityTypeConfiguration<CategoryTranslation>
{
    public void Configure(EntityTypeBuilder<CategoryTranslation> builder)
    {
        builder.ToTable("CategoryTranslation");

        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(60)
            .IsRequired();

        builder
            .Property(x => x.Slug)
            .HasVarcharMaxLength(150)
            .IsRequired();

        builder
            .HasOne(x => x.Category)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.CategoryId, x.LanguageId })
            .IsUnique();

        builder
            .HasIndex(x => new { x.LanguageId, x.Slug })
            .IsUnique();
    }
}

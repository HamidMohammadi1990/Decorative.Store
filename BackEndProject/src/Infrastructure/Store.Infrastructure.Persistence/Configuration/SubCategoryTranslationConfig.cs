using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class SubCategoryTranslationConfig : IEntityTypeConfiguration<SubCategoryTranslation>
{
    public void Configure(EntityTypeBuilder<SubCategoryTranslation> builder)
    {
        builder.ToTable("SubCategoryTranslation");

        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(60)
            .IsRequired();

        builder
            .Property(x => x.Slug)
            .HasVarcharMaxLength(150)
            .IsRequired();

        builder
            .HasOne(x => x.SubCategory)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.SubCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.SubCategoryId, x.LanguageId })
            .IsUnique();

        builder
            .HasIndex(x => new { x.LanguageId, x.Slug })
            .IsUnique();
    }
}

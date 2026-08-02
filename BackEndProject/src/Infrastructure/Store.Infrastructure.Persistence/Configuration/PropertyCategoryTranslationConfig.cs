using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class PropertyCategoryTranslationConfig : IEntityTypeConfiguration<PropertyCategoryTranslation>
{
    public void Configure(EntityTypeBuilder<PropertyCategoryTranslation> builder)
    {
        builder.ToTable("PropertyCategoryTranslation");

        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(30)
            .IsRequired();

        builder
            .HasOne(x => x.PropertyCategory)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.PropertyCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.PropertyCategoryId, x.LanguageId })
            .IsUnique();
    }
}

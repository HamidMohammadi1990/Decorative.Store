using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class PropertyItemTranslationConfig : IEntityTypeConfiguration<PropertyItemTranslation>
{
    public void Configure(EntityTypeBuilder<PropertyItemTranslation> builder)
    {
        builder.ToTable("PropertyItemTranslation");

        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(30)
            .IsRequired();

        builder
            .HasOne(x => x.PropertyItem)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.PropertyItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.PropertyItemId, x.LanguageId })
            .IsUnique();
    }
}

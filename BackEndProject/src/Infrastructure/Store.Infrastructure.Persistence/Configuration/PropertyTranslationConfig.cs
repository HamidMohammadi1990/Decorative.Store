using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class PropertyTranslationConfig : IEntityTypeConfiguration<PropertyTranslation>
{
    public void Configure(EntityTypeBuilder<PropertyTranslation> builder)
    {
        builder.ToTable("PropertyTranslation");

        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(30)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasNVarcharMaxLength(250);

        builder
            .HasOne(x => x.Property)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.PropertyId, x.LanguageId })
            .IsUnique();
    }
}

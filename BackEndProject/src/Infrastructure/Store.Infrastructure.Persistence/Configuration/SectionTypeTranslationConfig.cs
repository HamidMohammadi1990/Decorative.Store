using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class SectionTypeTranslationConfig : IEntityTypeConfiguration<SectionTypeTranslation>
{
    public void Configure(EntityTypeBuilder<SectionTypeTranslation> builder)
    {
        builder.ToTable("SectionTypeTranslation");

        builder.Property(x => x.Name).HasNVarcharMaxLength(80).IsRequired();

        builder
            .HasOne(x => x.SectionType)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.SectionTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.SectionTypeId, x.LanguageId }).IsUnique();
        builder.HasIndex(x => new { x.LanguageId, x.Name }).IsUnique();
    }
}

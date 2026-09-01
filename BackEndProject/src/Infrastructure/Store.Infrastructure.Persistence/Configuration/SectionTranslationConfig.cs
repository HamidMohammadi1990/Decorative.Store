using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class SectionTranslationConfig : IEntityTypeConfiguration<SectionTranslation>
{
    public void Configure(EntityTypeBuilder<SectionTranslation> builder)
    {
        builder.ToTable("SectionTranslation");

        builder.Property(x => x.Title).HasNVarcharMaxLength(300).IsRequired();
        builder.Property(x => x.Description).HasNVarcharMaxLength(500);
        builder.Property(x => x.Url).HasNVarcharMaxLength(200).IsRequired();

        builder
            .HasOne(x => x.Section)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.SectionId, x.LanguageId }).IsUnique();
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class SectionItemTranslationConfig : IEntityTypeConfiguration<SectionItemTranslation>
{
    public void Configure(EntityTypeBuilder<SectionItemTranslation> builder)
    {
        builder.ToTable("SectionItemTranslation");

        builder.Property(x => x.Title).HasNVarcharMaxLength(80).IsRequired();
        builder.Property(x => x.Description).HasNVarcharMaxLength(250);
        builder.Property(x => x.Url).HasNVarcharMaxLength(150);

        builder
            .HasOne(x => x.SectionItem)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.SectionItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.SectionItemId, x.LanguageId }).IsUnique();
    }
}

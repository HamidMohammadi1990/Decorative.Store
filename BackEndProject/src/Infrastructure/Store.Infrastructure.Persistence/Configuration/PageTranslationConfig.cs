using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class PageTranslationConfig : IEntityTypeConfiguration<PageTranslation>
{
    public void Configure(EntityTypeBuilder<PageTranslation> builder)
    {
        builder.ToTable("PageTranslation");

        builder.Property(x => x.Title).HasNVarcharMaxLength(60).IsRequired();
        builder.Property(x => x.Slug).HasNVarcharMaxLength(350).IsRequired();
        builder.Property(x => x.MetaTitle).HasNVarcharMaxLength(120);
        builder.Property(x => x.MetaDescription).HasNVarcharMaxLength(300);

        builder
            .HasOne(x => x.Page)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.PageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.PageId, x.LanguageId }).IsUnique();
        builder.HasIndex(x => new { x.LanguageId, x.Slug }).IsUnique();
    }
}

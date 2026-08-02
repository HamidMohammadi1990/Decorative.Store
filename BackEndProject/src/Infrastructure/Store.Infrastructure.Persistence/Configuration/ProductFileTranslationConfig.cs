using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ProductFileTranslationConfig : IEntityTypeConfiguration<ProductFileTranslation>
{
    public void Configure(EntityTypeBuilder<ProductFileTranslation> builder)
    {
        builder.ToTable("ProductFileTranslation");

        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(30)
            .IsRequired();

        builder
            .HasOne(x => x.ProductFile)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.ProductFileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.ProductFileId, x.LanguageId })
            .IsUnique();
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ProductTranslationConfig : IEntityTypeConfiguration<ProductTranslation>
{
    public void Configure(EntityTypeBuilder<ProductTranslation> builder)
    {
        builder.ToTable("ProductTranslation");

        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(150)
            .IsRequired();

        builder
            .Property(x => x.Slug)
            .HasVarcharMaxLength(150)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasNVarcharMaxLength(400)
            .IsRequired();

        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.ProductId, x.LanguageId })
            .IsUnique();

        builder
            .HasIndex(x => new { x.LanguageId, x.Slug })
            .IsUnique();
    }
}

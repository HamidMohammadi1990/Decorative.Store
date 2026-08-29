using Store.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ProductDescriptionConfig : IEntityTypeConfiguration<ProductDescription>
{
    public void Configure(EntityTypeBuilder<ProductDescription> builder)
    {
        builder
            .Property(x => x.Description)
            .HasNVarcharMaxLength(2500)
            .IsRequired();

        builder
            .HasOne(d => d.Product)
            .WithMany(p => p.ProductDescriptions)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(d => d.Language)
            .WithMany()
            .HasForeignKey(d => d.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.ProductId, x.LanguageId });
    }
}
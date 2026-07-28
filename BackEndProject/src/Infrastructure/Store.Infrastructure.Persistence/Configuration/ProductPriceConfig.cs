using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ProductPriceConfig : IEntityTypeConfiguration<ProductPrice>
{
    public void Configure(EntityTypeBuilder<ProductPrice> builder)
    {
        builder
            .Property(x => x.Price)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.CooperationPrice)
            .HasPrecision(18, 2);

        builder
            .HasOne(x => x.Company)
            .WithMany(x => x.ProductPrices)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(d => d.Product)
            .WithMany(p => p.ProductPrices)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.ProductPriceDeliveryOptions)
            .WithOne(x => x.ProductPrice)
            .HasForeignKey(x => x.ProductPriceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .ToTable(x => x.IsTemporal());

        builder
            .HasIndex(x => x.ProductId);

        builder
            .HasIndex(x => x.CompanyId);
    }
}
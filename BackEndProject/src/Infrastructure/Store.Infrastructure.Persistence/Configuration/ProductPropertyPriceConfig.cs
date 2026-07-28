using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ProductPropertyPriceConfig : IEntityTypeConfiguration<ProductPropertyPrice>
{
    public void Configure(EntityTypeBuilder<ProductPropertyPrice> builder)
    {
        builder
            .Property(x => x.Price)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.CooperationPrice)
            .HasPrecision(18, 2);

        builder
            .HasOne(x => x.Company)
            .WithMany(x => x.ProductPropertyPrices)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(d => d.ProductProperty)
            .WithOne(p => p.ProductPropertyPrice)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .ToTable(x => x.IsTemporal());

        builder
            .HasIndex(x => x.CompanyId);

        builder
            .HasIndex(x => x.ProductPropertyId);
    }
}
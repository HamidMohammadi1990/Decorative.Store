using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Enums;

namespace Store.Infrastructure.Persistence.Configuration;

public class ProductFeatureConfig : IEntityTypeConfiguration<ProductFeature>
{
    public void Configure(EntityTypeBuilder<ProductFeature> builder)
    {
        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.ProductFeatures)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.ProductFeatureType)
            .WithMany(x => x.ProductFeatures)
            .HasForeignKey(x => x.ProductFeatureTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.ProductId, x.ProductFeatureTypeId });
    }
}
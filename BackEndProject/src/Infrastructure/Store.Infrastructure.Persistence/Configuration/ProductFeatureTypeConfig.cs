using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class ProductFeatureTypeConfig : IEntityTypeConfiguration<ProductFeatureType>
{
    public void Configure(EntityTypeBuilder<ProductFeatureType> builder)
    {
        builder
            .Property(x => x.Name)
            .HasNVarcharMaxLength(50)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasVarcharMaxLength(100);

        builder
            .HasMany(x => x.ProductFeatures)
            .WithOne(x => x.ProductFeatureType)
            .HasForeignKey(x => x.ProductFeatureTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
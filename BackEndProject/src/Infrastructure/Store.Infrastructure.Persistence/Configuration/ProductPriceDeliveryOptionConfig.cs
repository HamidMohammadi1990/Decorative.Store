using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

public class ProductPriceDeliveryOptionConfig : IEntityTypeConfiguration<ProductPriceDeliveryOption>
{
    public void Configure(EntityTypeBuilder<ProductPriceDeliveryOption> builder)
    {
        builder
            .Property(x => x.Price)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.CooperationPrice)
            .HasPrecision(18, 2);

        builder
            .HasOne(x => x.ProductPrice)
            .WithMany(x => x.ProductPriceDeliveryOptions)
            .HasForeignKey(x => x.ProductPriceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.DeliveryOption)
            .WithMany(x => x.ProductPriceDeliveryOptions)
            .HasForeignKey(x => x.DeliveryOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
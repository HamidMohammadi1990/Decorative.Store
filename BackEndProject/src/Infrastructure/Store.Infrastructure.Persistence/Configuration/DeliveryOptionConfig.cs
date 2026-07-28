using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class DeliveryOptionConfig : IEntityTypeConfiguration<DeliveryOption>
{
    public void Configure(EntityTypeBuilder<DeliveryOption> builder)
    {
        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(30)
            .IsRequired();

        builder
            .HasMany(x => x.ProductPriceDeliveryOptions)
            .WithOne(x => x.DeliveryOption)
            .HasForeignKey(x => x.DeliveryOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
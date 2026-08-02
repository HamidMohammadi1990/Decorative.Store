using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

internal class PropertyItemPriceConfig : IEntityTypeConfiguration<PropertyItemPrice>
{
    public void Configure(EntityTypeBuilder<PropertyItemPrice> builder)
    {
        builder
            .Property(x => x.Price)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.CooperationPrice)
            .HasPrecision(18, 2);

        builder
            .HasOne(x => x.PropertyItem)
            .WithOne(x => x.PropertyItemPrice)            
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .ToTable(x => x.IsTemporal());

        builder
            .HasIndex(x => x.PropertyItemId);
    }
}
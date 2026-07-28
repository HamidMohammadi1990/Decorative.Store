using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class DeliveryTypeConfig : IEntityTypeConfiguration<DeliveryType>
{
    public void Configure(EntityTypeBuilder<DeliveryType> builder)
    {
        builder
           .Property(x => x.Title)
           .HasNVarcharMaxLength(30)
           .IsRequired();

        builder
            .HasMany(x => x.OrderItems)
            .WithOne(x => x.DeliveryType)
            .HasForeignKey(x => x.DeliveryTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
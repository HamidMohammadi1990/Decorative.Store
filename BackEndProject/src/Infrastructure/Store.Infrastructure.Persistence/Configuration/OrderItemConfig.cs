using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder
            .Property(x => x.ProductPrice)
            .HasPrecision(18, 2);

        builder
           .Property(x => x.Description)
           .HasNVarcharMaxLength(200);

        builder
            .Property(x => x.EmergencyPhoneNumber)
            .HasVarcharMaxLength(11);

        builder
            .HasOne(d => d.Order)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(d => d.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.OrderItemProperties)
            .WithOne(x => x.OrderItem)
            .HasForeignKey(x => x.OrderItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
           .HasOne(d => d.DeliveryType)
           .WithMany(p => p.OrderItems)
           .HasForeignKey(x => x.DeliveryTypeId)
           .IsRequired(false)
           .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(d => d.PostType)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(x => x.PostTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.UserAddress)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(x => x.UserAddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.OrderItemAttachments)
            .WithOne(x => x.OrderItem)
            .HasForeignKey(x => x.OrderItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => x.OrderId);

        builder
            .HasIndex(x => x.ProductId);
    }
}

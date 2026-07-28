using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class ProductOrderItemAttachmentTypeConfig : IEntityTypeConfiguration<ProductOrderItemAttachmentType>
{
    public void Configure(EntityTypeBuilder<ProductOrderItemAttachmentType> builder)
    {
        builder
            .Property(x => x.Description)
            .HasNVarcharMaxLength(200);

        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.ProductOrderItemAttachmentTypes)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.OrderItemAttachmentType)
            .WithMany(x => x.ProductOrderItemAttachmentTypes)
            .HasForeignKey(x => x.OrderItemAttachmentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.OrderItemAttachmentTypeRestrictions)
            .WithOne(x => x.ProductOrderItemAttachmentType)
            .HasForeignKey(x => x.ProductOrderItemAttachmentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.OrderItemAttachments)
            .WithOne(x => x.ProductOrderItemAttachmentType)
            .HasForeignKey(x => x.ProductOrderItemAttachmentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.ProductId);

        builder
            .HasIndex(x => x.OrderItemAttachmentTypeId);
    }
}
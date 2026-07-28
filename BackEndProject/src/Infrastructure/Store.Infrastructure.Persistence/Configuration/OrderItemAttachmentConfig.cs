using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class OrderItemAttachmentConfig : IEntityTypeConfiguration<OrderItemAttachment>
{
    public void Configure(EntityTypeBuilder<OrderItemAttachment> builder)
    {
        builder
            .Property(x => x.FileName)
            .HasVarcharMaxLength(50)
            .IsRequired();

        builder
            .HasOne(x => x.OrderItem)
            .WithMany(x => x.OrderItemAttachments)
            .HasForeignKey(x => x.OrderItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.ProductOrderItemAttachmentType)
            .WithMany(x => x.OrderItemAttachments)
            .HasForeignKey(x => x.ProductOrderItemAttachmentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.OrderItemId);

        builder
            .HasIndex(x => x.ProductOrderItemAttachmentTypeId);
    }
}
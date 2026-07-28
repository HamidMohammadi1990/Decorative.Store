using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

public class OrderItemAttachmentTypeRestrictionConfig : IEntityTypeConfiguration<OrderItemAttachmentTypeRestriction>
{
    public void Configure(EntityTypeBuilder<OrderItemAttachmentTypeRestriction> builder)
    {
        builder
            .Property(x => x.MinWidth)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.MaxWidth)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.MinHeight)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.MaxHeight)
            .HasPrecision(18, 2);

        builder
            .HasOne(x => x.ProductOrderItemAttachmentType)
            .WithMany(x => x.OrderItemAttachmentTypeRestrictions)
            .HasForeignKey(x => x.ProductOrderItemAttachmentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.ProductOrderItemAttachmentTypeId);
    }
}
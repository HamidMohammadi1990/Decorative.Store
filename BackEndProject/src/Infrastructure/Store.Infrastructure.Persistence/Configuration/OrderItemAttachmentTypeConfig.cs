using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class OrderItemAttachmentTypeConfig : IEntityTypeConfiguration<OrderItemAttachmentType>
{
    public void Configure(EntityTypeBuilder<OrderItemAttachmentType> builder)
    {
        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(50);

        builder
            .HasMany(x => x.ProductOrderItemAttachmentTypes)
            .WithOne(x => x.OrderItemAttachmentType)
            .HasForeignKey(x => x.OrderItemAttachmentTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
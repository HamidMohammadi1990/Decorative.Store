using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ProductPropertyConfig : IEntityTypeConfiguration<ProductProperty>
{
    public void Configure(EntityTypeBuilder<ProductProperty> builder)
    {
        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.ProductProperties)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(d => d.Property)
            .WithMany(p => p.ProductProperties)
            .HasForeignKey(x => x.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.PropertyItem)
            .WithMany(x => x.ProductProperties)
            .HasForeignKey(x => x.PropertyItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.ProductPropertyRule)
            .WithOne(x => x.ProductProperty)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.ProductPropertyPrice)
            .WithOne(x => x.ProductProperty)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.ProductId, x.PropertyId });
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class PropertyItemConfig : IEntityTypeConfiguration<PropertyItem>
{
    public void Configure(EntityTypeBuilder<PropertyItem> builder)
    {
        builder
            .Property(x => x.Code)
            .HasVarcharMaxLength(30)
            .IsRequired();

        builder
            .HasOne(d => d.Property)
            .WithMany(p => p.PropertyItems)
            .HasForeignKey(x => x.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.OrderItemProperties)
            .WithOne(x => x.PropertyItem)
            .HasForeignKey(x => x.PropertyItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.ParentPropertyItems)
            .WithOne(x => x.ParentPropertyItem)
            .HasForeignKey(x => x.ParentPropertyItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.DependentPropertyItems)
            .WithOne(x => x.DependentPropertyItem)
            .HasForeignKey(x => x.DependentPropertyItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.PropertyItemPrice)
            .WithOne(x => x.PropertyItem)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.PropertyId);

        builder
            .HasIndex(x => new { x.PropertyId, x.Code })
            .IsUnique();
    }
}

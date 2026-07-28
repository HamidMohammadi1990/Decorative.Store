using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class PropertyConfig : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(30)
            .IsRequired();

        builder
            .HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(d => d.PropertyCategory)
            .WithMany(p => p.Properties)
            .HasForeignKey(x => x.PropertyCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.ProductProperties)
            .WithOne(x => x.Property)
            .HasForeignKey(x => x.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.OrderItemProperties)
            .WithOne(x => x.Property)
            .HasForeignKey(x => x.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
           .HasMany(x => x.PropertyItems)
           .WithOne(x => x.Property)
           .HasForeignKey(x => x.PropertyId)
           .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(d => d.PropertyCategoryId);
    }
}
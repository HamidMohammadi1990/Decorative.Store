using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

internal class PropertyItemDependencyConfig : IEntityTypeConfiguration<PropertyItemDependency>
{
    public void Configure(EntityTypeBuilder<PropertyItemDependency> builder)
    {
        builder
            .HasKey(x => new { x.ParentPropertyItemId, x.DependentPropertyItemId });

        builder
            .HasOne(x => x.ParentPropertyItem)
            .WithMany(x => x.ParentPropertyItems)
            .HasForeignKey(x => x.ParentPropertyItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.DependentPropertyItem)
            .WithMany(x => x.DependentPropertyItems)
            .HasForeignKey(x => x.DependentPropertyItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
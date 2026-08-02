using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class PropertyCategoryConfig : IEntityTypeConfiguration<PropertyCategory>
{
    public void Configure(EntityTypeBuilder<PropertyCategory> builder)
    {
        builder
            .Property(x => x.Code)
            .HasVarcharMaxLength(20)
            .IsRequired();

        builder
            .HasMany(x => x.Properties)
            .WithOne(x => x.PropertyCategory)
            .HasForeignKey(x => x.PropertyCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.Code)
            .IsUnique();
    }
}

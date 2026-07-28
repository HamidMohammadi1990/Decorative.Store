using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ProductDescriptionConfig : IEntityTypeConfiguration<ProductDescription>
{
    public void Configure(EntityTypeBuilder<ProductDescription> builder)
    {
        builder
            .Property(x => x.Description)
            .HasNVarcharMaxLength(2500)
            .IsRequired();

        builder
            .HasOne(d => d.Product)
            .WithMany(p => p.ProductDescriptions)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.ProductId);
    }
}
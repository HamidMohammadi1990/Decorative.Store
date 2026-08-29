using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ProductFileConfig : IEntityTypeConfiguration<ProductFile>
{
    public void Configure(EntityTypeBuilder<ProductFile> builder)
    {
        builder
            .Property(x => x.FileName)
            .HasVarcharMaxLength(70)
            .IsRequired();

        builder
            .HasOne(d => d.Product)
            .WithMany(p => p.ProductFiles)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.ProductId);
    }
}

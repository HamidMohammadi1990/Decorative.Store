using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

public class CompanyProductConfig : IEntityTypeConfiguration<CompanyProduct>
{
    public void Configure(EntityTypeBuilder<CompanyProduct> builder)
    {
        builder.HasKey(x => new { x.ProductId, x.CompanyId });

        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.CompanyProducts)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Company)
            .WithMany(x => x.CompanyProducts)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
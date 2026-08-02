using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

public class OrderCommissionConfig : IEntityTypeConfiguration<OrderCommission>
{
    public void Configure(EntityTypeBuilder<OrderCommission> builder)
    {
        builder
            .Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder
            .HasOne(x => x.Order)
            .WithMany(x => x.OrderCommissions)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.FinancialDocument)
            .WithMany(x => x.OrderCommissions)
            .HasForeignKey(x => x.FinancialDocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.OrderId);

        builder
            .HasIndex(x => x.FinancialDocumentId);
    }
}
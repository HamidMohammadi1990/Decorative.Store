using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class PosTransactionConfig : IEntityTypeConfiguration<PosTransaction>
{
    public void Configure(EntityTypeBuilder<PosTransaction> builder)
    {
        builder
            .Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.Description)
            .HasNVarcharMaxLength(80)
            .IsRequired();

        builder
            .Property(x => x.CardNumber)
            .HasVarcharMaxLength(16);

        builder
            .Property(x => x.TransactionNumber)
            .HasVarcharMaxLength(15)
            .IsRequired();

        builder
            .HasOne(x => x.FinancialDocument)
            .WithMany(x => x.PosTransactions)
            .HasForeignKey(x => x.FinancialDocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.CompanyPosDevice)
            .WithMany(x => x.PosTransactions)
            .HasForeignKey(x => x.CompanyPosDeviceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.CompanyPosDeviceId);

        builder
            .HasIndex(x => x.FinancialDocumentId);
    }
}
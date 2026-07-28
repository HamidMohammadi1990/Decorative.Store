using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class ChequeTransactionConfig : IEntityTypeConfiguration<ChequeTransaction>
{
    public void Configure(EntityTypeBuilder<ChequeTransaction> builder)
    {
        builder
            .Property(x => x.FileName)
            .HasVarcharMaxLength(40);

        builder
            .Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.CheckNumber)
            .HasVarcharMaxLength(15)
            .IsRequired();

        builder
            .Property(x => x.SayadTrackingNumber)
            .HasVarcharMaxLength(15)
            .IsRequired();

        builder
            .HasOne(x => x.Bank)
            .WithMany(x => x.ChequeTransactions)
            .HasForeignKey(x => x.BankId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.ChequeTransactions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Company)
            .WithMany(x => x.ChequeTransactions)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.FinancialDocument)
            .WithMany(x => x.ChequeTransactions)
            .HasForeignKey(x => x.FinancialDocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Expenses)
            .WithOne(x => x.ChequeTransaction)
            .HasForeignKey(x => x.ChequeTransactionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.UserId);

        builder
            .HasIndex(x => x.CompanyId);

        builder
            .HasIndex(x => x.FinancialDocumentId);

        builder
            .HasIndex(x => x.BankId);
    }
}
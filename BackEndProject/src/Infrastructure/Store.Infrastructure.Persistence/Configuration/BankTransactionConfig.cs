using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class BankTransactionConfig : IEntityTypeConfiguration<BankTransaction>
{
    public void Configure(EntityTypeBuilder<BankTransaction> builder)
    {
        builder
           .Property(x => x.TransactionNumber)
           .HasVarcharMaxLength(50)
           .IsRequired();

        builder
            .Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder
           .Property(x => x.Description)
           .HasNVarcharMaxLength(80)
           .IsRequired();

        builder
           .HasOne(x => x.User)
           .WithMany(x => x.BankTransactions)
           .HasForeignKey(x => x.UserId)
           .OnDelete(DeleteBehavior.Restrict);

        builder
           .HasOne(x => x.Company)
           .WithMany(x => x.BankTransactions)
           .HasForeignKey(x => x.CompanyId)
           .OnDelete(DeleteBehavior.Restrict);

        builder
           .HasOne(x => x.BankAccount)
           .WithMany(x => x.BankTransactions)
           .HasForeignKey(x => x.BankAccountId)
           .OnDelete(DeleteBehavior.Restrict);

        builder
           .HasOne(x => x.FinancialDocument)
           .WithMany(x => x.BankTransactions)
           .HasForeignKey(x => x.FinancialDocumentId)
           .OnDelete(DeleteBehavior.Restrict);

        builder
           .HasMany(x => x.Expenses)
           .WithOne(x => x.BankTransaction)
           .HasForeignKey(x => x.BankTransactionId)
           .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.FinancialDocumentId);

        builder
            .HasIndex(x => x.BankAccountId);

        builder
            .HasIndex(x => x.UserId);

        builder
            .HasIndex(x => x.CompanyId);
    }
}
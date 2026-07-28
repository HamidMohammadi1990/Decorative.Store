using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class ExpenseConfig : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder
            .Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.Description)
            .HasNVarcharMaxLength(250)
            .IsRequired();

        builder
            .HasOne(x => x.ExpenseType)
            .WithMany(x => x.Expenses)
            .HasForeignKey(x => x.ExpenseTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.BankTransaction)
            .WithMany(x => x.Expenses)
            .HasForeignKey(x => x.BankTransactionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.ChequeTransaction)
            .WithMany(x => x.Expenses)
            .HasForeignKey(x => x.ChequeTransactionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.FinancialDocument)
            .WithMany(x => x.Expenses)
            .HasForeignKey(x => x.FinancialDocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.WalletTransaction)
            .WithMany(x => x.Expenses)
            .HasForeignKey(x => x.WalletTransactionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.FinancialDocumentId);

        builder
            .HasIndex(x => x.ExpenseTypeId);
    }
}
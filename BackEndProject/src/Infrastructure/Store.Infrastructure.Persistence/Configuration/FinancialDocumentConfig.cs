using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class FinancialDocumentConfig : IEntityTypeConfiguration<FinancialDocument>
{
    public void Configure(EntityTypeBuilder<FinancialDocument> builder)
    {
        builder
            .Property(x => x.DocumentNumber)
            .HasVarcharMaxLength(15);

        builder
            .Property(x => x.Description)
            .HasNVarcharMaxLength(150)
            .IsRequired();

        builder
            .HasOne(x => x.Order)
            .WithMany(x => x.FinancialDocuments)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.FinancialYear)
            .WithMany(x => x.FinancialDocuments)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.RefundedFinancialDocument)
            .WithMany(x => x.RefundedFinancialDocuments)
            .HasForeignKey(x => x.RelatedRefundDocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Expenses)
            .WithOne(x => x.FinancialDocument)
            .HasForeignKey(x => x.FinancialDocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.OrderVats)
            .WithOne(x => x.FinancialDocument)
            .HasForeignKey(x => x.FinancialDocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.BankTransactions)
            .WithOne(x => x.FinancialDocument)
            .HasForeignKey(x => x.FinancialDocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.OrderCommissions)
            .WithOne(x => x.FinancialDocument)
            .HasForeignKey(x => x.FinancialDocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.WalletTransactions)
            .WithOne(x => x.FinancialDocument)
            .HasForeignKey(x => x.FinancialDocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.ChequeTransactions)
            .WithOne(x => x.FinancialDocument)
            .HasForeignKey(x => x.FinancialDocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.FinancialDocumentDetails)
            .WithOne(x => x.FinancialDocument)
            .HasForeignKey(x => x.FinancialDocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.FinancialYearId);

        builder
            .HasIndex(x => x.OrderId);
    }
}
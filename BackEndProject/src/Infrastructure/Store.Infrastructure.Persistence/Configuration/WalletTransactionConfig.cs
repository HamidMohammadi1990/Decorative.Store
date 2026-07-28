using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class WalletTransactionConfig : IEntityTypeConfiguration<WalletTransaction>
{
    public void Configure(EntityTypeBuilder<WalletTransaction> builder)
    {
        builder
            .Property(x => x.Description)
            .HasNVarcharMaxLength(80)
            .IsRequired();

        builder
            .Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.WalletTransactions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Wallet)
            .WithMany(x => x.WalletTransactions)
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Company)
            .WithMany(x => x.WalletTransactions)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.DestinationWallet)
            .WithMany(x => x.DestinationWalletTransactions)
            .HasForeignKey(x => x.DestinationWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.FinancialDocument)
            .WithMany(x => x.WalletTransactions)
            .HasForeignKey(x => x.FinancialDocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Expenses)
            .WithOne(x => x.WalletTransaction)
            .HasForeignKey(x => x.WalletTransactionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.UserId);

        builder
            .HasIndex(x => x.CompanyId);

        builder
            .HasIndex(x => x.FinancialDocumentId);

        builder
            .HasIndex(x => x.WalletId);

        builder
            .HasIndex(x => x.DestinationWalletId);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class WalletConfig : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(30)
            .IsRequired();

        builder
           .Property(x => x.Balance)
           .HasPrecision(18, 2);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Wallets)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Company)
            .WithMany(x => x.Wallets)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.WalletTransactions)
            .WithOne(x => x.Wallet)
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.DestinationWalletTransactions)
            .WithOne(x => x.DestinationWallet)
            .HasForeignKey(x => x.DestinationWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.UserId);

        builder
            .HasIndex(x => x.CompanyId);
    }
}
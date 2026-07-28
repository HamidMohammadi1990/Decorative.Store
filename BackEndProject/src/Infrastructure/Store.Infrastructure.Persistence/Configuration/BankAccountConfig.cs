using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class BankAccountConfig : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(50)
            .IsRequired();

        builder
            .Property(x => x.PaymentUrl)
            .HasVarcharMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.VerifyPaymentUrl)
            .HasVarcharMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.SuccessCallBackUrl)
            .HasVarcharMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.FailureCallBackUrl)
            .HasVarcharMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.MerchantCode)
            .HasVarcharMaxLength(50);

        builder
            .Property(x => x.ApiKey)
            .HasVarcharMaxLength(50);

        builder
            .Property(x => x.Dscription)
            .HasNVarcharMaxLength(150);

        builder
            .HasOne(x => x.Bank)
            .WithMany(x => x.BankAccounts)
            .HasForeignKey(x => x.BankId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.BankTransactions)
            .WithOne(x => x.BankAccount)
            .HasForeignKey(x => x.BankAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.BankId);
    }
}
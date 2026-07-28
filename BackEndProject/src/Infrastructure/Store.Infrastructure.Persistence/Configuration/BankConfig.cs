using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class BankConfig : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(30)
            .IsRequired();

        builder
            .Property(x => x.Icon)
            .HasVarcharMaxLength(50)
            .IsRequired();

        builder
            .HasMany(x => x.BankAccounts)
            .WithOne(x => x.Bank)
            .HasForeignKey(x => x.BankId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.CompanyPosDevices)
            .WithOne(x => x.Bank)
            .HasForeignKey(x => x.BankId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.ChequeTransactions)
            .WithOne(x => x.Bank)
            .HasForeignKey(x => x.BankId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class ChartOfAccountConfig : IEntityTypeConfiguration<ChartOfAccount>
{
    public void Configure(EntityTypeBuilder<ChartOfAccount> builder)
    {
        builder
            .Property(x => x.AccountCode)
            .HasVarcharMaxLength(20)
            .IsRequired();

        builder
            .Property(x => x.AccountTitle)
            .HasNVarcharMaxLength(50)
            .IsRequired();

        builder
            .HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.FinancialDocumentDetails)
            .WithOne(x => x.ChartOfAccount)
            .HasForeignKey(x => x.ChartOfAccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
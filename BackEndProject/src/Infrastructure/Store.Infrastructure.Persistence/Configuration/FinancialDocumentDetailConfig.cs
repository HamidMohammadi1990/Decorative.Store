using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class FinancialDocumentDetailConfig : IEntityTypeConfiguration<FinancialDocumentDetail>
{
    public void Configure(EntityTypeBuilder<FinancialDocumentDetail> builder)
    {
        builder
            .Property(x => x.Debit)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.Credit)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.Description)
            .HasNVarcharMaxLength(100)
            .IsRequired();

        builder
            .HasOne(x => x.ChartOfAccount)
            .WithMany(x => x.FinancialDocumentDetails)
            .HasForeignKey(x => x.ChartOfAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.FinancialDocument)
            .WithMany(x => x.FinancialDocumentDetails)
            .HasForeignKey(x => x.FinancialDocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.ChartOfAccountId);

        builder
            .HasIndex(x => x.FinancialDocumentId);
    }
}
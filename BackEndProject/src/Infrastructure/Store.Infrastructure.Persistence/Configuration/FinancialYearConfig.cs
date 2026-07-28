using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class FinancialYearConfig : IEntityTypeConfiguration<FinancialYear>
{
    public void Configure(EntityTypeBuilder<FinancialYear> builder)
    {
        builder
            .Property(x => x.Name)
            .HasNVarcharMaxLength(50)
            .IsRequired();

        builder
            .HasOne(x => x.Company)
            .WithMany(x => x.FinancialYears)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.FinancialDocuments)
            .WithOne(x => x.FinancialYear)
            .HasForeignKey(x => x.FinancialYearId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.CompanyId);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class CurrencyConfig : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder
            .Property(x => x.Code)
            .IsRequired()
            .HasVarcharMaxLength(5);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasNVarcharMaxLength(15);
    }
}
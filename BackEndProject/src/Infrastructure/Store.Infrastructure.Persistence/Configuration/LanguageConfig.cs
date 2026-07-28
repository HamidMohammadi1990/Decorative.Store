using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class LanguageConfig : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("Language");

        builder
            .Property(x => x.Code)
            .HasVarcharMaxLength(10)
            .IsRequired();

        builder
            .Property(x => x.Name)
            .HasNVarcharMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Code).IsUnique();

        builder
            .HasIndex(x => x.IsDefault)
            .IsUnique()
            .HasFilter($"[{nameof(Language.IsDefault)}] = 1");
    }
}

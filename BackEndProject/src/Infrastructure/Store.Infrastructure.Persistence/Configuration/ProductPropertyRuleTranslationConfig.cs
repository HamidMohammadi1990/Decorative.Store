using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ProductPropertyRuleTranslationConfig : IEntityTypeConfiguration<ProductPropertyRuleTranslation>
{
    public void Configure(EntityTypeBuilder<ProductPropertyRuleTranslation> builder)
    {
        builder.ToTable("ProductPropertyRuleTranslation");

        builder
            .Property(x => x.Description)
            .HasNVarcharMaxLength(250);

        builder
            .HasOne(x => x.ProductPropertyRule)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.ProductPropertyRuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.ProductPropertyRuleId, x.LanguageId })
            .IsUnique();
    }
}

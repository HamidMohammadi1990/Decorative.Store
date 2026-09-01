using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class MarketingStripDisclaimerConfig : IEntityTypeConfiguration<MarketingStripDisclaimer>
{
    public void Configure(EntityTypeBuilder<MarketingStripDisclaimer> builder)
    {
        builder
            .Property(x => x.Disclaimer)
            .HasNVarcharMaxLength(500);

        builder
            .Property(x => x.DisclaimerLinkLabel)
            .HasNVarcharMaxLength(50);

        builder
            .Property(x => x.DisclaimerLinkHref)
            .HasNVarcharMaxLength(200);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.LanguageId)
            .IsUnique();
    }
}

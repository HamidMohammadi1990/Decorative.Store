using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class MarketingPromoConfig : IEntityTypeConfiguration<MarketingPromo>
{
    public void Configure(EntityTypeBuilder<MarketingPromo> builder)
    {
        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.Subtitle)
            .HasNVarcharMaxLength(200);

        builder
            .Property(x => x.LinkLabel)
            .HasNVarcharMaxLength(50)
            .IsRequired();

        builder
            .Property(x => x.LinkHref)
            .HasNVarcharMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.ImageFileName)
            .HasNVarcharMaxLength(35)
            .IsRequired();

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.LanguageId, x.Priority });
    }
}

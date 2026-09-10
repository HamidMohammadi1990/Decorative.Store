using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class SectionItemConfig : IEntityTypeConfiguration<SectionItem>
{
    public void Configure(EntityTypeBuilder<SectionItem> builder)
    {
        builder
            .Property(x => x.AdminDescription)
            .HasNVarcharMaxLength(500);

        builder
            .Property(x => x.Icon)
            .HasVarcharMaxLength(24);

        builder
            .Property(x => x.ImageUrl)
            .HasVarcharMaxLength(256);

        builder
            .HasOne(x => x.Section)
            .WithMany(x => x.SectionItems)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Translations)
            .WithOne(x => x.SectionItem)
            .HasForeignKey(x => x.SectionItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

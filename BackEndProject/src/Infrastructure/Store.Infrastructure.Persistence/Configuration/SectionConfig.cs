using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class SectionConfig : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder
            .Property(x => x.AdminDescription)
            .HasNVarcharMaxLength(500);

        builder
            .Property(x => x.ImageUrl)
            .HasVarcharMaxLength(80);

        builder
            .HasOne(x => x.SectionType)
            .WithMany(x => x.Sections)
            .HasForeignKey(x => x.SectionTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.SectionItems)
            .WithOne(x => x.Section)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.PageSections)
            .WithOne(x => x.Section)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Translations)
            .WithOne(x => x.Section)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

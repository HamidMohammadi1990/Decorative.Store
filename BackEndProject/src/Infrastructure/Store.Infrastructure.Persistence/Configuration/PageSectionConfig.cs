using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

public class PageSectionConfig : IEntityTypeConfiguration<PageSection>
{
    public void Configure(EntityTypeBuilder<PageSection> builder)
    {
        builder
            .HasOne(x => x.Page)
            .WithMany(x => x.PageSections)
            .HasForeignKey(x => x.PageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Section)
            .WithMany(x => x.PageSections)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.PageId, x.SectionId })
            .IsUnique();
    }
}
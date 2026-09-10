using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class PageConfig : IEntityTypeConfiguration<Page>
{
    public void Configure(EntityTypeBuilder<Page> builder)
    {
        builder
            .Property(x => x.AdminDescription)
            .HasNVarcharMaxLength(500);

        builder
            .HasMany(x => x.PageSections)
            .WithOne(x => x.Page)
            .HasForeignKey(x => x.PageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Translations)
            .WithOne(x => x.Page)
            .HasForeignKey(x => x.PageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

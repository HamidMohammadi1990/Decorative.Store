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
            .Property(x => x.Slug)
            .HasNVarcharMaxLength(350)
            .IsRequired();

        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(60)
            .IsRequired();

        builder
            .Property(x => x.MetaTitle)
            .HasNVarcharMaxLength(120);

        builder
            .Property(x => x.MetaDescription)
            .HasNVarcharMaxLength(300);

        builder
            .HasMany(x => x.PageSections)
            .WithOne(x => x.Page)
            .HasForeignKey(x => x.PageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.Slug)
            .IsUnique();
    }
}
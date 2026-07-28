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
            .Property(x => x.Title)
            .HasNVarcharMaxLength(80)
            .IsRequired();

        builder
            .Property(x => x.Icon)
            .HasVarcharMaxLength(24);

        builder
            .Property(x => x.ImageUrl)
            .HasVarcharMaxLength(36);

        builder
            .Property(x => x.Url)
            .HasNVarcharMaxLength(150);

        builder
            .Property(x => x.Description)
            .HasNVarcharMaxLength(250);

        builder
            .HasOne(x => x.Section)
            .WithMany(x => x.SectionItems)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
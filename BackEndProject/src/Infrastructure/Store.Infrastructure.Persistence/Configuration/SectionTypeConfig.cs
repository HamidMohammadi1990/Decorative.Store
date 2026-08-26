using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

public class SectionTypeConfig : IEntityTypeConfiguration<SectionType>
{
    public void Configure(EntityTypeBuilder<SectionType> builder)
    {
        builder
            .HasMany(x => x.Sections)
            .WithOne(x => x.SectionType)
            .HasForeignKey(x => x.SectionTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Translations)
            .WithOne(x => x.SectionType)
            .HasForeignKey(x => x.SectionTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

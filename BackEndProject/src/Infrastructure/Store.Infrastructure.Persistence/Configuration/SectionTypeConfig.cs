using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class SectionTypeConfig : IEntityTypeConfiguration<SectionType>
{
    public void Configure(EntityTypeBuilder<SectionType> builder)
    {
        builder
            .Property(x => x.Name)
            .HasNVarcharMaxLength(80)
            .IsRequired();

        builder
            .HasMany(x => x.Sections)
            .WithOne(x => x.SectionType)
            .HasForeignKey(x => x.SectionTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.Name)
            .IsUnique();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ProvinceConfig : IEntityTypeConfiguration<Province>
{
    public void Configure(EntityTypeBuilder<Province> builder)
    {
        builder
            .Property(e => e.Name)
            .HasNVarcharMaxLength(25)
            .IsRequired();

        builder
            .Property(x => x.Slug)
            .HasNVarcharMaxLength(30)
            .IsRequired();

        builder
            .Property(x => x.TelPrefix)
            .HasVarcharMaxLength(6);

        builder
            .Property(e => e.Description)
            .HasNVarcharMaxLength(200);

        builder
            .HasMany(x => x.Cities)
            .WithOne(x => x.Province)
            .HasForeignKey(x => x.ProvinceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.Slug)
            .IsUnique();
    }
}
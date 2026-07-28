using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class CityConfig : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder
            .Property(x => x.Name)
            .HasNVarcharMaxLength(25)
            .IsRequired();

        builder
            .Property(x => x.Slug)
            .HasNVarcharMaxLength(30)
            .IsRequired();

        builder
            .Property(e => e.Description)
            .HasNVarcharMaxLength(200);

        builder
            .HasOne(x => x.Province)
            .WithMany(x => x.Cities)
            .HasForeignKey(x => x.ProvinceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Users)
            .WithOne(x => x.City)
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Companies)
            .WithOne(x => x.City)
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.UserAddresses)
            .WithOne(x => x.City)
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.ProvinceId);

        builder
            .HasIndex(x => x.Slug)
            .IsUnique();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class UserAddressConfig : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        builder
           .Property(x => x.Title)
           .HasNVarcharMaxLength(30)
           .IsRequired();

        builder
            .Property(x => x.RecipientFirstName)
            .HasNVarcharMaxLength(20);

        builder
            .Property(x => x.RecipientLastName)
            .HasNVarcharMaxLength(30);

        builder
            .Property(x => x.PostalCode)
            .HasVarcharMaxLength(10);

        builder
            .Property(x => x.Address)
            .HasNVarcharMaxLength(150)
            .IsRequired();

        builder
            .Property(x => x.Apartment)
            .HasNVarcharMaxLength(50);

        builder
            .Property(x => x.PhoneNumber)
            .HasVarcharMaxLength(11);

        builder
            .Property(x => x.Latitude)
            .HasPrecision(10, 7);

        builder
            .Property(x => x.Longitude)
            .HasPrecision(10, 7);

        builder
            .Property(x => x.IsDefault)
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.UserAddresses)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.City)
            .WithMany(x => x.UserAddresses)
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.OrderItems)
            .WithOne(x => x.UserAddress)
            .HasForeignKey(x => x.UserAddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
           .HasIndex(x => x.UserId);

        builder
            .HasIndex(x => x.CityId);

        builder
            .HasIndex(x => new { x.UserId, x.IsDefault });
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder
            .Property(x => x.TotalPrice)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.FinalPrice)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.VatPrice)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.TotalCommissionPrice)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.DiscountAmount)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.TrackingCode)
            .IsRequired();

        builder
          .Property(x => x.Title)
          .HasNVarcharMaxLength(50)
          .IsRequired();

        builder
          .HasOne(d => d.User)
          .WithMany(p => p.Orders)
          .HasForeignKey(x => x.UserId)
          .OnDelete(DeleteBehavior.Restrict);

        builder
          .HasMany(d => d.OrderVats)
          .WithOne(p => p.Order)
          .HasForeignKey(x => x.OrderId)
          .OnDelete(DeleteBehavior.Restrict);

        builder
          .HasMany(d => d.OrderItems)
          .WithOne(p => p.Order)
          .HasForeignKey(x => x.OrderId)
          .OnDelete(DeleteBehavior.Restrict);

        builder
          .HasMany(x => x.OrderNotes)
          .WithOne(x => x.Order)
          .HasForeignKey(x => x.OrderId)
          .OnDelete(DeleteBehavior.Restrict);

        builder
          .HasMany(x => x.OrderCommissions)
          .WithOne(x => x.Order)
          .HasForeignKey(x => x.OrderId)
          .OnDelete(DeleteBehavior.Restrict);

        builder
          .HasMany(x => x.FinancialDocuments)
          .WithOne(x => x.Order)
          .HasForeignKey(x => x.OrderId)
          .OnDelete(DeleteBehavior.Restrict);

        builder
          .HasOne(x => x.AppliedDiscount)
          .WithMany()
          .HasForeignKey(x => x.AppliedDiscountId)
          .OnDelete(DeleteBehavior.Restrict);

        builder
          .HasIndex(x => x.UserId);

        builder
          .HasIndex(x => x.TrackingCode)
          .IsUnique();
    }
}

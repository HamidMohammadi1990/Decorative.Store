using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class DiscountConfig : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder
            .Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.MaxDiscountAmount)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.MinimumAmount)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.Code)
            .HasVarcharMaxLength(20)
            .IsRequired();

        builder
            .HasOne(d => d.SubCategory)
            .WithMany(p => p.Discounts)
            .HasForeignKey(x => x.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(d => d.Product)
            .WithMany(p => p.ProductDiscounts)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.ProductDiscounts)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.Code)
            .IsUnique();
    }
}
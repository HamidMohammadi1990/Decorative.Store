using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ProductWishlistConfig : IEntityTypeConfiguration<ProductWishlist>
{
    public void Configure(EntityTypeBuilder<ProductWishlist> builder)
    {
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.ProductWishlists)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.ProductWishlists)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.UserId, x.ProductId })
            .IsUnique();

        builder
            .HasIndex(x => x.UserId);
    }
}

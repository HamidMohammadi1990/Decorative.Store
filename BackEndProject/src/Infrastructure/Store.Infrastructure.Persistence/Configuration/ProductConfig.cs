using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(150)
            .IsRequired();

        builder
            .Property(x => x.Slug)
            .HasVarcharMaxLength(150)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasNVarcharMaxLength(400)
            .IsRequired();

        builder
            .Property(x => x.ProductCode)
            .HasVarcharMaxLength(10)
            .IsRequired();

        builder
            .HasOne(x => x.SubCategory)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.OrderItems)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
           .HasMany(x => x.ProductComments)
           .WithOne(x => x.Product)
           .HasForeignKey(x => x.ProductId)
           .OnDelete(DeleteBehavior.Restrict);

        builder
           .HasMany(x => x.ProductDescriptions)
           .WithOne(x => x.Product)
           .HasForeignKey(x => x.ProductId)
           .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.ProductFiles)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.ProductPrices)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.ProductProperties)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.ProductDiscounts)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.CompanyProducts)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.ProductOrderItemAttachmentTypes)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.ProductFeatures)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.Title)
            .IsUnique();

        builder
            .HasIndex(x => x.Slug)
            .IsUnique();

        builder
            .HasIndex(x => x.SubCategoryId);
    }
}
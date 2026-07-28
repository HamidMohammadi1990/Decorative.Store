using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class SubCategoryConfig : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder
           .Property(x => x.Title)
           .HasNVarcharMaxLength(60)
           .IsRequired();

        builder
            .Property(x => x.Slug)
            .HasVarcharMaxLength(150)
            .IsRequired();

        builder
            .Property(x => x.Code)
            .HasVarcharMaxLength(12);

        builder
            .HasOne(d => d.Category)
            .WithMany(p => p.SubCategories)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Discounts)
            .WithOne(x => x.SubCategory)
            .HasForeignKey(x => x.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Products)
            .WithOne(x => x.SubCategory)
            .HasForeignKey(x => x.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.Slug)
            .IsUnique();

        builder
            .HasIndex(x => x.CategoryId);
    }
}
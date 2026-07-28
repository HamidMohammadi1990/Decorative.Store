using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class CategoryConfig : IEntityTypeConfiguration<Category>
{
	public void Configure(EntityTypeBuilder<Category> builder)
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
			.HasMany(x => x.SubCategories)
			.WithOne(x => x.Category)
			.HasForeignKey(x => x.CategoryId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.Property(x => x.Code)
			.HasVarcharMaxLength(12);

		builder
			.HasIndex(x => x.Slug)
			.IsUnique();
	}
}
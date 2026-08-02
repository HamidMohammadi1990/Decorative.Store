using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Infrastructure.Persistence.Extensions;
using Store.Domain.Enums;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ProductPropertyRuleConfig : IEntityTypeConfiguration<ProductPropertyRule>
{
	public void Configure(EntityTypeBuilder<ProductPropertyRule> builder)
	{
		builder
			.HasDiscriminator(x => x.PropertyType)
			.HasValue<ProductPropertyRule>(PropertyType.Select)
			.HasValue<ProductPropertyRule>(PropertyType.Boolean)
			.HasValue<TextProductPropertyRule>(PropertyType.Text)
			.HasValue<NumericProductPropertyRule>(PropertyType.Numeric)
			.HasValue<NumericProductPropertyRule>(PropertyType.NumericWithItem)
			.HasValue<DimensionsProductPropertyRule>(PropertyType.Dimensions);

		builder
			.HasOne(x => x.ProductProperty)
			.WithOne(x => x.ProductPropertyRule)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasIndex(x => x.ProductPropertyId);
	}
}

internal class NumericProductPropertyRuleConfig : IEntityTypeConfiguration<NumericProductPropertyRule>
{
	public void Configure(EntityTypeBuilder<NumericProductPropertyRule> builder)
	{
		builder
		   .Property(x => x.MinQuantity)
		   .HasPrecision(18, 2);

		builder
			.Property(x => x.MaxQuantity)
			.HasPrecision(18, 2);
	}
}

internal class DimensionsProductPropertyRuleConfig : IEntityTypeConfiguration<DimensionsProductPropertyRule>
{
	public void Configure(EntityTypeBuilder<DimensionsProductPropertyRule> builder)
	{
		builder
			.Property(x => x.MinWidth)
			.HasPrecision(18, 2);

		builder
			.Property(x => x.MaxWidth)
			.HasPrecision(18, 2);

		builder
			.Property(x => x.MinHeight)
			.HasPrecision(18, 2);

		builder
			.Property(x => x.MaxHeight)
			.HasPrecision(18, 2);
	}
}

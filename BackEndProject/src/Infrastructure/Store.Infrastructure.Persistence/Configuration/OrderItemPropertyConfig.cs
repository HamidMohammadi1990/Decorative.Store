using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Infrastructure.Persistence.Extensions;
using Store.Domain.Enums;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

internal class OrderItemPropertyConfig : IEntityTypeConfiguration<OrderItemProperty>
{
	public void Configure(EntityTypeBuilder<OrderItemProperty> builder)
	{
		builder
			.Property(x => x.PropertyPrice)
			.HasPrecision(18, 2);

		builder
			.Property(x => x.PropertyItemPrice)
			.HasPrecision(18, 2);

		builder
			.HasOne(x => x.OrderItem)
			.WithMany(x => x.OrderItemProperties)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(x => x.Property)
			.WithMany(x => x.OrderItemProperties)
			.HasForeignKey(x => x.PropertyId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(x => x.PropertyItem)
			.WithMany(x => x.OrderItemProperties)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasDiscriminator(x => x.PropertyType)
			.HasValue<OrderItemProperty>(PropertyType.Select)
			.HasValue<TextOrderItemProperty>(PropertyType.Text)
			.HasValue<BooleanOrderItemProperty>(PropertyType.Boolean)
			.HasValue<NumericOrderItemProperty>(PropertyType.Numeric)
			.HasValue<DimensionsOrderItemProperty>(PropertyType.Dimensions)
			.HasValue<NumericOrderItemProperty>(PropertyType.NumericWithItem);

		builder
			.HasIndex(x => x.OrderItemId);

		builder
			.HasIndex(x => x.PropertyId);

		builder
			.HasIndex(x => x.PropertyItemId);
	}
}

internal class DimensionsOrderItemPropertyConfig : IEntityTypeConfiguration<DimensionsOrderItemProperty>
{
	public void Configure(EntityTypeBuilder<DimensionsOrderItemProperty> builder)
	{
		builder
			.Property(x => x.Width)
			.HasPrecision(18, 2);

		builder
			.Property(x => x.Height)
			.HasPrecision(18, 2);
	}
}

internal class TextOrderItemPropertyConfig : IEntityTypeConfiguration<TextOrderItemProperty>
{
	public void Configure(EntityTypeBuilder<TextOrderItemProperty> builder)
	{
		builder
			.Property(x => x.Value)
			.HasNVarcharMaxLength(80);
	}
}
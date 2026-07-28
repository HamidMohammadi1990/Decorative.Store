using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class OrderItemProperty : BaseEntity
{
	public int OrderItemId { get; private set; }
	public int PropertyId { get; protected set; }
	public decimal PropertyPrice { get; private set; }
	public int? PropertyItemId { get; protected set; }
	public decimal PropertyItemPrice { get; private set; }
	public PropertyType PropertyType { get; protected set; }
	public bool IsActive { get; private set; } = true;


	public Property? Property { get; private set; } = default!;
	public OrderItem? OrderItem { get; private set; } = default!;
	public PropertyItem? PropertyItem { get; private set; } = default!;


	public static OrderItemProperty Create(int propertyId, int propertyItemId, PropertyType propertyType)
		=> new()
		{
			PropertyId = propertyId,
			PropertyType = propertyType,
			PropertyItemId = propertyItemId
		};

	public void SetPropertyPrice(decimal propertyPrice)
	{
		PropertyPrice = propertyPrice;
	}

	public void SetPropertyItemPrice(decimal propertyItemPrice)
	{
		PropertyItemPrice = propertyItemPrice;
	}
}

public class BooleanOrderItemProperty : OrderItemProperty
{
	public bool IsSelected { get; private set; }


	public static BooleanOrderItemProperty Create(int propertyId, bool isSelected, PropertyType propertyType)
		=> new()
		{
			PropertyId = propertyId,
			IsSelected = isSelected,
			PropertyType = propertyType
		};
}

public class NumericOrderItemProperty : OrderItemProperty
{
	public int Quantity { get; private set; }


	public static NumericOrderItemProperty Create(int propertyId, int quantity, PropertyType propertyType)
		=> new()
		{
			Quantity = quantity,
			PropertyId = propertyId,
			PropertyType = propertyType
		};

	public static NumericOrderItemProperty Create(int propertyId, int propertyItemId, int quantity, PropertyType propertyType)
		=> new()
		{
			Quantity = quantity,
			PropertyId = propertyId,
			PropertyType = propertyType,
			PropertyItemId = propertyItemId
		};
}

public class DimensionsOrderItemProperty : OrderItemProperty
{
	public decimal Width { get; private set; }
	public decimal Height { get; private set; }


	public static DimensionsOrderItemProperty Create(int propertyId, decimal width, decimal height, PropertyType propertyType)
		=> new()
		{
			Width = width,
			Height = height,
			PropertyId = propertyId,
			PropertyType = propertyType
		};
}

public class TextOrderItemProperty : OrderItemProperty
{
	public string? Value { get; set; }


	public static TextOrderItemProperty Create(int propertyId, PropertyType propertyType, string? value)
		=> new()
		{
			Value = value,
			PropertyId = propertyId,
			PropertyType = propertyType
		};
}
using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public abstract class ProductPropertyRule : BaseEntity
{
	public bool IsMandatory { get; private set; }
	public string? Description { get; private set; }
	public int ProductPropertyId { get; private set; }
	public PropertyType PropertyType { get; private set; }
	public bool IsActive { get; private set; } = true;


	public ProductProperty ProductProperty { get; private set; } = default!;

	public static ProductPropertyRule Create(
		bool isMandatory,
		string? description,
		int productPropertyId,
		PropertyType propertyType,
		bool isActive,
		int? minLength = null,
		int? maxLength = null,
		decimal? minQuantity = null,
		decimal? maxQuantity = null,
		decimal? minWidth = null,
		decimal? maxWidth = null,
		decimal? minHeight = null,
		decimal? maxHeight = null)
	{
		return propertyType switch
		{
			PropertyType.Text => new TextProductPropertyRule
			{
				IsMandatory = isMandatory,
				Description = description,
				ProductPropertyId = productPropertyId,
				PropertyType = propertyType,
				IsActive = isActive,
				MinLength = minLength ?? 0,
				MaxLength = maxLength ?? 0
			},
			PropertyType.Numeric or PropertyType.NumericWithItem => new NumericProductPropertyRule
			{
				IsMandatory = isMandatory,
				Description = description,
				ProductPropertyId = productPropertyId,
				PropertyType = propertyType,
				IsActive = isActive,
				MinQuantity = minQuantity ?? 0,
				MaxQuantity = maxQuantity ?? 0
			},
			PropertyType.Dimensions => new DimensionsProductPropertyRule
			{
				IsMandatory = isMandatory,
				Description = description,
				ProductPropertyId = productPropertyId,
				PropertyType = propertyType,
				IsActive = isActive,
				MinWidth = minWidth ?? 0,
				MaxWidth = maxWidth ?? 0,
				MinHeight = minHeight ?? 0,
				MaxHeight = maxHeight ?? 0
			},
			_ => throw new InvalidOperationException("Only Text, Numeric and Dimensions property rule types are supported.")
		};
	}

	public void Update(
		bool isMandatory,
		string? description,
		int productPropertyId,
		bool isActive,
		int? minLength = null,
		int? maxLength = null,
		decimal? minQuantity = null,
		decimal? maxQuantity = null,
		decimal? minWidth = null,
		decimal? maxWidth = null,
		decimal? minHeight = null,
		decimal? maxHeight = null)
	{
		IsMandatory = isMandatory;
		Description = description;
		ProductPropertyId = productPropertyId;
		IsActive = isActive;

		switch (this)
		{
			case TextProductPropertyRule textRule:
				textRule.MinLength = minLength ?? 0;
				textRule.MaxLength = maxLength ?? 0;
				break;
			case NumericProductPropertyRule numericRule:
				numericRule.MinQuantity = minQuantity ?? 0;
				numericRule.MaxQuantity = maxQuantity ?? 0;
				break;
			case DimensionsProductPropertyRule dimensionsRule:
				dimensionsRule.MinWidth = minWidth ?? 0;
				dimensionsRule.MaxWidth = maxWidth ?? 0;
				dimensionsRule.MinHeight = minHeight ?? 0;
				dimensionsRule.MaxHeight = maxHeight ?? 0;
				break;
		}
	}

	public ProductPropertyRulePayload Get()
	{
		return this switch
		{
			TextProductPropertyRule textRule => new ProductPropertyRulePayload
			{
				MinLength = textRule.MinLength,
				MaxLength = textRule.MaxLength
			},
			NumericProductPropertyRule numericRule => new ProductPropertyRulePayload
			{
				MinQuantity = numericRule.MinQuantity,
				MaxQuantity = numericRule.MaxQuantity
			},
			DimensionsProductPropertyRule dimensionsRule => new ProductPropertyRulePayload
			{
				MinWidth = dimensionsRule.MinWidth,
				MaxWidth = dimensionsRule.MaxWidth,
				MinHeight = dimensionsRule.MinHeight,
				MaxHeight = dimensionsRule.MaxHeight
			},
			_ => new ProductPropertyRulePayload()
		};
	}
}

public class TextProductPropertyRule : ProductPropertyRule
{
	public int MinLength { get; set; }
	public int MaxLength { get; set; }
}

public class NumericProductPropertyRule : ProductPropertyRule
{
	public decimal MinQuantity { get; set; }
	public decimal MaxQuantity { get; set; }
}

public class DimensionsProductPropertyRule : ProductPropertyRule
{
	public decimal MinWidth { get; set; }
	public decimal MaxWidth { get; set; }
	public decimal MinHeight { get; set; }
	public decimal MaxHeight { get; set; }
}

public record ProductPropertyRulePayload
{
	public int? MinLength { get; init; }
	public int? MaxLength { get; init; }
	public decimal? MinQuantity { get; init; }
	public decimal? MaxQuantity { get; init; }
	public decimal? MinWidth { get; init; }
	public decimal? MaxWidth { get; init; }
	public decimal? MinHeight { get; init; }
	public decimal? MaxHeight { get; init; }
}
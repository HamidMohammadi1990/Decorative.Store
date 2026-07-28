using Store.Domain.Enums;

namespace Store.Domain.Dtos.Products;

public class ProductPropertyDto
{
	public int ProductPropertyId { get; set; }
	public int? ParentProductPropertyId { get; set; }
	public string CategoryTitle { get; set; } = null!;
	public int PropertyId { get; set; }
	public string PropertyTitle { get; set; } = null!;
	public int PropertyPriority { get; set; }
	public PropertyType PropertyType { get; set; }
	public int? PropertyParentId { get; set; }
	public int? PropertyPriceId { get; set; }
	public decimal? PropertyPrice { get; set; } = default!;
	public decimal? PropertyCooperationPrice { get; set; } = default!;
	public int? ParentPropertyId { get; set; }
	public string? ParentPropertyTitle { get; set; }
	public int? ParentPropertyPriority { get; set; }
	public PropertyType? ParentPropertyType { get; set; }
	public int? ParentPropertyPriceId { get; set; }
	public decimal? ParentPropertyPrice { get; set; } = default!;
	public decimal? ParentPropertyCooperationPrice { get; set; } = default!;
	public int? PropertyItemId { get; set; }
	public int? PropertyItemPropertyId { get; set; }
	public string? PropertyItemTitle { get; set; }
	public int? PropertyItemPriority { get; set; }
	public int? PropertyItemPriceId { get; set; }
	public decimal? PropertyItemPrice { get; set; } = default!;
	public decimal? PropertyItemCooperationPrice { get; set; } = default!;
	public int? DependencyParentPropertyItemId { get; set; }
	public int? DependencyDependentPropertyItemId { get; set; }
	public int? ParentPropertyItemId { get; set; }
	public int? ParentPropertyItemPropertyId { get; set; }
	public string? ParentPropertyItemTitle { get; set; }
	public int? ParentPropertyItemPriority { get; set; }
	public int? ParentPropertyItemPriceId { get; set; }
	public decimal? ParentPropertyItemPrice { get; set; } = default!;
	public decimal? ParentPropertyItemCooperationPrice { get; set; } = default!;
	public int? PropertyRuleProductPropertyId { get; set; }
	public bool? PropertyRuleIsMandatory { get; set; }
	public string? PropertyRuleDescription { get; set; }
	public PropertyType? PropertyRulePropertyType { get; set; }
	public decimal? PropertyRuleMinQuantity { get; set; }
	public decimal? PropertyRuleMaxQuantity { get; set; }
	public decimal? PropertyRuleMinWidth { get; set; }
	public decimal? PropertyRuleMaxWidth { get; set; }
	public decimal? PropertyRuleMinHeight { get; set; }
	public decimal? PropertyRuleMaxHeight { get; set; }
	public int? PropertyRuleMinLength { get; set; }
	public int? PropertyRuleMaxLength { get; set; }
	public int? ParentPropertyRuleProductPropertyId { get; set; }
	public bool? ParentPropertyRuleIsMandatory { get; set; }
	public string? ParentPropertyRuleDescription { get; set; }
	public PropertyType? ParentPropertyRulePropertyType { get; set; }
	public decimal? ParentPropertyRuleMinQuantity { get; set; }
	public decimal? ParentPropertyRuleMaxQuantity { get; set; }
	public decimal? ParentPropertyRuleMinWidth { get; set; }
	public decimal? ParentPropertyRuleMaxWidth { get; set; }
	public decimal? ParentPropertyRuleMinHeight { get; set; }
	public decimal? ParentPropertyRuleMaxHeight { get; set; }
	public int? ParentPropertyRuleMinLength { get; set; }
	public int? ParentPropertyRuleMaxLength { get; set; }
}
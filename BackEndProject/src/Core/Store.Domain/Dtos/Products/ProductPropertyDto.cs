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
	public int? ParentPropertyId { get; set; }
	public string? ParentPropertyTitle { get; set; }
	public int? ParentPropertyPriority { get; set; }
	public PropertyType? ParentPropertyType { get; set; }
	public int? PropertyItemId { get; set; }
	public int? PropertyItemPropertyId { get; set; }
	public string? PropertyItemTitle { get; set; }
	public int? PropertyItemPriority { get; set; }
	public int? DependencyParentPropertyItemId { get; set; }
	public int? DependencyDependentPropertyItemId { get; set; }
	public int? ParentPropertyItemId { get; set; }
	public int? ParentPropertyItemPropertyId { get; set; }
	public string? ParentPropertyItemTitle { get; set; }
	public int? ParentPropertyItemPriority { get; set; }
}

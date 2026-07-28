using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class Property : BaseEntity
{
    public int? ParentId { get; private set; }
    public string Title { get; private set; } = default!;
    public int PropertyCategoryId { get; private set; }
    public int Priority { get; private set; }
    public PropertyType PropertyType { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string? Description { get; private set; }


    public Property Parent { get; private set; } = default!;
    public List<Property> Children { get; private set; } = default!;
    public PropertyCategory PropertyCategory { get; private set; } = default!;
    public ICollection<PropertyItem> PropertyItems { get; private set; } = default!;
    public ICollection<ProductProperty> ProductProperties { get; private set; } = default!;
    public ICollection<OrderItemProperty> OrderItemProperties { get; private set; } = default!;


    public static Property Create(PropertyType propertyType, int? parentId, string title, int propertyCategoryId, int priority, string? description)
        => new()
        {
            Title = title,
            ParentId = parentId,
            Priority = priority,
            Description = description,
            PropertyType = propertyType,
            PropertyCategoryId = propertyCategoryId,
        };

    public void DeActive()
    {
        IsActive = false;
    }

    public void Update(PropertyType propertyType, int? parentId, string title, int propertyCategoryId, int priority, bool isActive)
    {
        Title = title;
        IsActive = isActive;
        ParentId = parentId;
        Priority = priority;
        PropertyType = propertyType;
        PropertyCategoryId = propertyCategoryId;
    }
}
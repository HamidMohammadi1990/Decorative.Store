using Store.Domain.Common;

namespace Store.Domain.Entities;

public class PropertyItem : BaseEntity
{
    public string Title { get; private set; } = default!;
    public int PropertyId { get; private set; }
    public int Priority { get; private set; }
    public bool IsActive { get; private set; } = true;


    public Property Property { get; private set; } = default!;
    public PropertyItemPrice PropertyItemPrice { get; private set; } = default!;
    public ICollection<OrderItemProperty> OrderItemProperties { get; private set; } = default!;
    public ICollection<PropertyItemDependency> ParentPropertyItems { get; private set; } = default!;
    public ICollection<PropertyItemDependency> DependentPropertyItems { get; private set; } = default!;


    public static PropertyItem Create(string title, int propertyId, int priority)
        => new()
        {
            Title = title,
            PropertyId = propertyId,
            Priority = priority
        };

    public void DeActive()
    {
        IsActive = false;
    }

    public void Update(string title, int propertyId, bool isActive, int priority)
    {
        Title = title;
        IsActive = isActive;
        PropertyId = propertyId;
        Priority = priority;
    }
}
using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductProperty : BaseEntity
{
    public int ProductId { get; private set; }
    public int PropertyId { get; private set; }
    public int? PropertyItemId { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Product Product { get; private set; } = default!;
    public Property Property { get; private set; } = default!;
    public PropertyItem? PropertyItem { get; private set; }

    public static ProductProperty Create(int productId, int propertyId, bool isActive, int? propertyItemId = null)
        => new()
        {
            ProductId = productId,
            PropertyId = propertyId,
            PropertyItemId = propertyItemId,
            IsActive = isActive
        };

    public void Update(int productId, int propertyId, bool isActive, int? propertyItemId = null)
    {
        ProductId = productId;
        PropertyId = propertyId;
        PropertyItemId = propertyItemId;
        IsActive = isActive;
    }
}

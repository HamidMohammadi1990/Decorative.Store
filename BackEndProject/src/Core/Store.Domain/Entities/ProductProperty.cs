using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductProperty : BaseEntity
{
    public int ProductId { get; private set; }
    public int PropertyId { get; private set; }
    public bool IsActive { get; private set; } = true;


    public Product Product { get; private set; } = default!;
    public Property Property { get; private set; } = default!;
    public ProductPropertyRule ProductPropertyRule { get; private set; } = default!;
    public ProductPropertyPrice ProductPropertyPrice { get; private set; } = default!;

    public static ProductProperty Create(int productId, int propertyId, bool isActive)
        => new()
        {
            ProductId = productId,
            PropertyId = propertyId,
            IsActive = isActive
        };

    public void Update(int productId, int propertyId, bool isActive)
    {
        ProductId = productId;
        PropertyId = propertyId;
        IsActive = isActive;
    }
}
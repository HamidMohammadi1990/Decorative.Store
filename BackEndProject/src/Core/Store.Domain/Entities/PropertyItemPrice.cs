using Store.Domain.Common;

namespace Store.Domain.Entities;

public class PropertyItemPrice : BaseEntity
{
    public int PropertyItemId { get; private set; }
    public decimal Price { get; private set; }
    public decimal CooperationPrice { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public bool IsActive { get; private set; } = true;

    public PropertyItem PropertyItem { get; private set; } = default!;

    public static PropertyItemPrice Create(int propertyItemId, decimal price, decimal cooperationPrice)
        => new()
        {
            Price = price,
            PropertyItemId = propertyItemId,
            CooperationPrice = cooperationPrice
        };

    public void Update(decimal price, decimal cooperationPrice, int propertyItemId, bool isActive)
    {
        Price = price;
        IsActive = isActive;
        PropertyItemId = propertyItemId;
        CooperationPrice = cooperationPrice;
    }
}

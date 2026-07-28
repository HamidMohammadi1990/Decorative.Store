using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductPropertyPrice : BaseEntity
{
    public int CompanyId { get; private set; }
    public int ProductPropertyId { get; private set; }
    public decimal Price { get; private set; }
    public decimal CooperationPrice { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public bool IsActive { get; private set; } = true;


    public Company Company { get; private set; } = default!;
    public ProductProperty ProductProperty { get; private set; } = default!;


    public static ProductPropertyPrice Create(int companyId, int productPropertyId, decimal price, decimal cooperationPrice)
        => new()
        {
            Price = price,
            CompanyId = companyId,
            CooperationPrice = cooperationPrice,
            ProductPropertyId = productPropertyId
        };
    public void Update(decimal price, decimal cooperationPrice, bool isActive)
    {
        Price = price;
        IsActive = isActive;
        CooperationPrice = cooperationPrice;
    }
}
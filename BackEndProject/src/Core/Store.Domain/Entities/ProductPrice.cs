using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductPrice : BaseEntity
{
    public int CompanyId { get; private set; }
    public int ProductId { get; private set; }
    public decimal Price { get; private set; }
    public decimal CooperationPrice { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public bool IsActive { get; private set; } = true;


    public Company Company { get; private set; } = default!;
    public Product Product { get; private set; } = default!;
    public ICollection<ProductPriceDeliveryOption> ProductPriceDeliveryOptions { get; private set; } = default!;


    public static ProductPrice Create(decimal price, decimal cooperationPrice, int productid, int companyId)
        => new()
        {
            Price = price,
            CooperationPrice = cooperationPrice,
            ProductId = productid,
            CompanyId = companyId
        };
    public void Update(decimal price, decimal cooperationPrice, bool isActive)
    {
        Price = price;
        IsActive = isActive;
        CooperationPrice = cooperationPrice;
    }
}
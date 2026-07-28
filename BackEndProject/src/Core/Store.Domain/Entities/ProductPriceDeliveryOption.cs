using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductPriceDeliveryOption : BaseEntity
{
    public int ProductPriceId { get; private set; }
    public int DeliveryOptionId { get; private set; }
    public decimal Price { get; private set; }
    public decimal CooperationPrice { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public bool IsActive { get; private set; } = true;


    public static ProductPriceDeliveryOption Create(int productPriceId, int deliveryOptionId, decimal price, decimal cooperationPrice)
        => new()
        {
            Price = price,
            ProductPriceId = productPriceId,
            CooperationPrice = cooperationPrice,
            DeliveryOptionId = deliveryOptionId
        };

    public void Update(int productPriceId, int deliveryOptionId, decimal price, decimal cooperationPrice)
    {
        Price = price;
        ProductPriceId = productPriceId;
        CooperationPrice = cooperationPrice;
        DeliveryOptionId = deliveryOptionId;
    }

    public ProductPrice ProductPrice { get; private set; } = default!;
    public DeliveryOption DeliveryOption { get; private set; } = default!;
}
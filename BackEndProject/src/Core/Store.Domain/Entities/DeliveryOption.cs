using Store.Domain.Common;

namespace Store.Domain.Entities;

public class DeliveryOption : BaseEntity
{
    public string Title { get; private set; } = default!;
    public int DeliveryDays { get; private set; }
    public bool IsActive { get; private set; } = true;


    public ICollection<ProductPriceDeliveryOption> ProductPriceDeliveryOptions { get; private set; } = default!;


    public static DeliveryOption Create(string title, int deliveryDays)
        => new()
        {
            Title = title,
            DeliveryDays = deliveryDays
        };

    public void Update(string title, int deliveryDays)
    {
        Title = title;
        DeliveryDays = deliveryDays;
    }
}
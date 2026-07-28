using Store.Domain.Common;

namespace Store.Domain.Entities;

public class DeliveryType : BaseEntity
{
    public string Title { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    public int Priority { get; private set; }


    public ICollection<OrderItem> OrderItems { get; private set; } = default!;


    public static DeliveryType Create(string title, int priority)
        => new()
        {
            Title = title,
            Priority = priority
        };

    public void Update(string title, int priority, bool isActive)
    {
        Title = title;
        Priority = priority;
        IsActive = isActive;
    }
}
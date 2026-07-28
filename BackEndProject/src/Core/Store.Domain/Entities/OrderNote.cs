using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class OrderNote : BaseEntity
{
    public string Title { get; private set; } = default!;
    public int OrderId { get; private set; }
    public bool IsVisibleToCustomer { get; set; }
    public int CreatedByUserId { get; private set; }
    public OrderNoteType Type { get; private set; }
    public string Description { get; private set; } = default!;
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;


    public User User { get; private set; } = default!;
    public Order Order { get; private set; } = default!;


    public static OrderNote Create(string title, int orderId, bool isVisibleToCustomer, int createdByUserId, OrderNoteType type, string description)
        => new()
        {
            Type = type,
            Title = title,
            OrderId = orderId,
            Description = description,
            CreatedByUserId = createdByUserId,
            IsVisibleToCustomer = isVisibleToCustomer
        };
}
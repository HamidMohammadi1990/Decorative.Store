using Store.Domain.Common;

namespace Store.Domain.Entities;

public class OrderItemAttachmentType : BaseEntity
{
    public string Title { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;


    public ICollection<ProductOrderItemAttachmentType> ProductOrderItemAttachmentTypes { get; private set; } = default!;


    public static OrderItemAttachmentType Create(string title)
        => new()
        {
            Title = title
        };
}
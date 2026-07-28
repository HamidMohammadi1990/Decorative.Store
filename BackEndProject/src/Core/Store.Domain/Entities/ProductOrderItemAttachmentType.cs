using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductOrderItemAttachmentType : BaseEntity
{
    public int ProductId { get; private set; }
    public string? Description { get; private set; }
    public int OrderItemAttachmentTypeId { get; private set; }
    public int Priority { get; private set; }    


    public Product Product { get; private set; } = default!;
    public OrderItemAttachmentType OrderItemAttachmentType { get; private set; } = default!;
    public ICollection<OrderItemAttachment> OrderItemAttachments { get; private set; } = default!;
    public ICollection<OrderItemAttachmentTypeRestriction> OrderItemAttachmentTypeRestrictions { get; private set; } = default!;


    public static ProductOrderItemAttachmentType Create(int productId, string? description, int orderItemAttachmentTypeId, int priority)
        => new()
        {
            Priority = priority,
            ProductId = productId,            
            Description = description,
            OrderItemAttachmentTypeId = orderItemAttachmentTypeId
        };

    public void Update(int productId, string? description, int orderItemAttachmentTypeId, int priority)
    {
        ProductId = productId;
        Description = description;
        OrderItemAttachmentTypeId = orderItemAttachmentTypeId;
        Priority = priority;
    }
}
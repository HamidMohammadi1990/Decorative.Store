using Store.Domain.Common;

namespace Store.Domain.Entities;

public class OrderItemAttachment : BaseEntity
{
    public int OrderItemId { get; private set; }
    public int ProductOrderItemAttachmentTypeId { get; private set; }
    public string FileName { get; private set; } = default!;
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;


    public OrderItem OrderItem { get; private set; } = default!;
    public ProductOrderItemAttachmentType ProductOrderItemAttachmentType { get; private set; } = default!;


    public static OrderItemAttachment Create(string fileName, int productOrderItemAttachmentTypeId)
        => new()
        {
            FileName = fileName,
            ProductOrderItemAttachmentTypeId = productOrderItemAttachmentTypeId,
        };
}
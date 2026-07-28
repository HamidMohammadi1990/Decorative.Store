using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int OrderId { get; private set; }
    public int Quantity { get; private set; }
    public int ProductId { get; private set; }
    public int CompanyId { get; private set; }
    public decimal ProductPrice { get; private set; }
    public bool IsNeedToDesign { get; private set; }
    public int DeliveryTypeId { get; private set; }
    public int? PostTypeId { get; private set; }
    public int? UserAddressId { get; private set; }
    public OrderItemStatusType Status { get; private set; } = OrderItemStatusType.Pending;
    public string? EmergencyPhoneNumber { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;


    public Order Order { get; private set; } = default!;
    public Product Product { get; private set; } = default!;
    public Company Company { get; private set; } = default!;
    public PostType PostType { get; private set; } = default!;
    public UserAddress UserAddress { get; private set; } = default!;
    public DeliveryType DeliveryType { get; private set; } = default!;
    public ICollection<OrderItemProperty> OrderItemProperties { get; private set; } = default!;
    public ICollection<OrderItemAttachment> OrderItemAttachments { get; private set; } = default!;


    public static OrderItem Create(int productId, int companyId, int quantity, int? postTypeId, int deliveryTypeId, int userAddressId,
                                   string? description, string? emergencyPhoneNumber, decimal productPrice, bool isNeedToDesign)
        => new()
        {
            ProductId = productId,
            CompanyId = companyId,
            PostTypeId = postTypeId,
            DeliveryTypeId = deliveryTypeId,
            UserAddressId = userAddressId,
            Description = description,
            EmergencyPhoneNumber = emergencyPhoneNumber,
            ProductPrice = productPrice,
            IsNeedToDesign = isNeedToDesign,
            Quantity = quantity,
        };

    public void AddProperties(List<OrderItemProperty> orderItemProperties)
    {
        foreach (var orderItemProperty in orderItemProperties)
            OrderItemProperties.Add(orderItemProperty);
    }
    public void AddAttachments(List<OrderItemAttachment> orderAttachments)
    {
        foreach (var orderAttachment in orderAttachments)
            OrderItemAttachments.Add(orderAttachment);
    }
    public decimal GetSumPrices()
    {
        var sumPrices = ProductPrice;
        foreach (var orderitemProperty in OrderItemProperties)
        {
            if (orderitemProperty is BooleanOrderItemProperty booleanProperty)
            {
                sumPrices += booleanProperty.PropertyPrice;
            }
            else if (orderitemProperty is NumericOrderItemProperty numericProperty)
            {
                if (numericProperty.PropertyType is PropertyType.Numeric)
                {
                    sumPrices += numericProperty.PropertyPrice * numericProperty.Quantity;
                }

                if (numericProperty.PropertyType is PropertyType.NumericWithItem)
                {
                    sumPrices += (numericProperty.PropertyPrice * numericProperty.Quantity) + numericProperty.PropertyItemPrice;
                }
            }
            else if (orderitemProperty is DimensionsOrderItemProperty dimensionsProperty)
            {
                sumPrices += (dimensionsProperty.Height * dimensionsProperty.Width) * dimensionsProperty.PropertyPrice;
            }
        }
        return sumPrices;
    }
}
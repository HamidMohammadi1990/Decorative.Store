using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Commands;

public record PurchaseOrderRequest : IRequest<OperationResult<PurchaseOrderResponse>>
{
    public string Title { get; init; } = null!;

    public int Quantity { get; init; }

    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    [JsonConverter(typeof(UserAddressEncryptor))]
    public int UserAddressId { get; init; }

    public string? EmergencyPhoneNumber { get; init; }

    public string? Description { get; init; }

    [JsonConverter(typeof(PostTypeEncryptor))]
    public int? PostTypeId { get; init; }
    public bool IsNeedToDesign { get; init; }
    public List<BaseOrderProperty> Properties { get; init; } = null!;
    public List<OrderAttachmentRequest>? Attachments { get; init; }
}

[JsonDerivedType(typeof(TextOrderProperty), (int)PropertyType.Text)]
[JsonDerivedType(typeof(SelectOrderProperty), (int)PropertyType.Select)]
[JsonDerivedType(typeof(BooleanOrderProperty), (int)PropertyType.Boolean)]
[JsonDerivedType(typeof(NumericOrderProperty), (int)PropertyType.Numeric)]
[JsonDerivedType(typeof(HasParentsOrderProperty), (int)PropertyType.HasParents)]
[JsonDerivedType(typeof(DimensionsOrderProperty), (int)PropertyType.Dimensions)]
[JsonDerivedType(typeof(NumericWithItemOrderProperty), (int)PropertyType.NumericWithItem)]
public record BaseOrderProperty
{
    [JsonConverter(typeof(PropertyEncryptor))]
    public int Id { get; init; }
    public PropertyType PropertyType { get; init; }
}

public record BooleanOrderProperty : BaseOrderProperty
{
    public bool IsSelected { get; init; }
}

public record HasParentsOrderProperty : BaseOrderProperty
{
    public List<BaseOrderProperty>? Properties { get; init; }
}

public record NumericOrderProperty : BaseOrderProperty
{
    public int Quantity { get; init; }
}

public record NumericWithItemOrderProperty : BaseOrderProperty
{
    public int Quantity { get; init; }

    [JsonConverter(typeof(PropertyItemEncryptor))]
    public int ItemId { get; init; }
}

public record DimensionsOrderProperty : BaseOrderProperty
{
    public decimal Width { get; init; }
    public decimal Height { get; init; }
}

public record SelectOrderProperty : BaseOrderProperty
{
    [JsonConverter(typeof(PropertyItemEncryptor))]
    public int ItemId { get; init; }
}

public record TextOrderProperty : BaseOrderProperty
{
    public string? Value { get; set; }
}

public record OrderAttachmentRequest
{
    public int Id { get; init; }
    public IFormFile File { get; init; } = default!;
}
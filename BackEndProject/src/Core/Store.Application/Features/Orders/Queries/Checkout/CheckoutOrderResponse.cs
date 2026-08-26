using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Enums;
using Store.Domain.Dtos.Others;

namespace Edition.Application.Features.Orders.Queries;

public record CheckoutOrderResponse
{
    public CheckoutProductResponse Product { get; init; } = default!;
    public List<CheckoutPropertiesResponse> Properties { get; init; } = default!;
    public List<CheckoutPostTypeResponse> PostTypes { get; init; } = default!;
    public List<CheckoutUserAddressResponse> Addresses { get; init; } = default!;
    public List<CheckoutOrderAttachmentResponse> Attachments { get; init; } = default!;
}

public record CheckoutProductResponse
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string ProductCode { get; init; } = null!;
    public string SubCategoryTitle { get; init; } = null!;
    public List<CheckoutProductImageResponse> Images { get; init; } = [];
}

public record CheckoutProductImageResponse
{
    public string Title { get; init; } = null!;
    public string Url { get; init; } = null!;
}

public record CheckoutPostTypeResponse
{
    [JsonConverter(typeof(PostTypeEncryptor))]
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
}

public record CheckoutPropertiesResponse
{
    public string CategoryTitle { get; init; } = null!;
    public List<CheckoutPropertyResponse> Properties { get; init; } = [];
}

public record CheckoutPropertItemDependencyResponse
{
    [JsonConverter(typeof(PropertyItemEncryptor))]
    public int ParentId { get; init; }

    [JsonConverter(typeof(PropertyItemEncryptor))]
    public int DependentId { get; init; }
}

public record CheckoutPropertyItemResponse
{
    [JsonConverter(typeof(PropertyItemEncryptor))]
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public PriceField Price { get; init; } = default!;
}

public record CheckoutPropertyResponse
{
    [JsonConverter(typeof(PropertyEncryptor))]
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public PropertyType PropertyType { get; init; }

    [JsonConverter(typeof(PropertyEncryptor))]
    public int? DependentPropertyId { get; init; }

    public PriceField Price { get; init; } = default!;
    public List<CheckoutPropertyItemResponse> Items { get; set; } = [];
    public List<CheckoutPropertItemDependencyResponse> Dependencies { get; set; } = [];
    public List<CheckoutPropertyResponse> Parents { get; set; } = [];
}

public record CheckoutUserAddressResponse
{
    [JsonConverter(typeof(UserAddressEncryptor))]
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public string Address { get; init; } = null!;
}

public record CheckoutOrderAttachmentResponse
{
    [JsonConverter(typeof(OrderItemAttachmentEncryptor))]
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public CheckoutOrderAttachmentRestrictionResponse Restriction { get; init; } = default!;
}

public record CheckoutOrderAttachmentRestrictionResponse
{
    public bool IsRequired { get; init; }
    public decimal MinWidth { get; init; }
    public decimal MaxWidth { get; init; }
    public decimal MinHeight { get; init; }
    public decimal MaxHeight { get; init; }
    public int MinHorizontalResolution { get; init; }
    public int MaxHorizontalResolution { get; init; }
    public int MinVerticalResolution { get; init; }
    public int MaxVerticalResolution { get; init; }
    public PictureColorModeType ColorMode { get; init; }
    public int MaxFileSizeInMegaBytes { get; init; }
}
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductPrices.Queries;

public record GetAllProductPriceResponse
{
    [JsonConverter(typeof(ProductPriceEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; }

    public string CompanyName { get; init; } = default!;

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    public string UserFirstName { get; init; } = default!;
    public string UserLastName { get; init; } = default!;

    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    public string ProductTitle { get; init; } = default!;
    public decimal Price { get; init; }
    public decimal CooperationPrice { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public bool IsActive { get; init; }
}
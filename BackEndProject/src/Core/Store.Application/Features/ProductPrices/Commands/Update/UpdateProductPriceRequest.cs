using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductPrices.Commands;

public record UpdateProductPriceRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductPriceEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; }

    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    public decimal Price { get; init; }
    public decimal CooperationPrice { get; init; }
    public bool IsActive { get; init; }
}
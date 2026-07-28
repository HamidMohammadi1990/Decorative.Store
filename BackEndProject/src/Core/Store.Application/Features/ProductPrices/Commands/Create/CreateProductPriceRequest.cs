using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductPrices.Commands;

public record CreateProductPriceRequest : IRequest<OperationResult<CreateProductPriceResponse>>
{
    public decimal Price { get; init; }
    public decimal CooperationPrice { get; init; }

    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; }
}
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductPropertyPrices.Commands;

public record CreateProductPropertyPriceRequest : IRequest<OperationResult<CreateProductPropertyPriceResponse>>
{
    [JsonConverter(typeof(ProductPropertyEncryptor))]
    public int ProductPropertyId { get; init; }

    public decimal Price { get; init; }
    public decimal CooperationPrice { get; init; }
}

using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.PropertyItemPrices.Commands;

public record CreatePropertyItemPriceRequest : IRequest<OperationResult<CreatePropertyItemPriceResponse>>
{
    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; }

    [JsonConverter(typeof(PropertyItemEncryptor))]
    public int PropertyItemId { get; init; }

    public decimal Price { get; init; }
    public decimal CooperationPrice { get; init; }
}
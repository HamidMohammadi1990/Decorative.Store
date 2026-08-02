using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.PropertyItemPrices.Commands;

public record UpdatePropertyItemPriceRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(PropertyItemPriceEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(PropertyItemEncryptor))]
    public int PropertyItemId { get; init; }

    public decimal Price { get; init; }
    public decimal CooperationPrice { get; init; }
    public bool IsActive { get; init; }
}

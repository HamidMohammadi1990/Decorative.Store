using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductPropertyPrices.Commands;

public record UpdateProductPropertyPriceRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductPropertyPriceEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; }

    [JsonConverter(typeof(ProductPropertyEncryptor))]
    public int ProductPropertyId { get; init; }

    public decimal Price { get; init; }
    public decimal CooperationPrice { get; init; }
    public bool IsActive { get; init; }
}
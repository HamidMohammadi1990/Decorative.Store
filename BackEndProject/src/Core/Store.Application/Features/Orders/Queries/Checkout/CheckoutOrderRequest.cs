using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Orders.Queries;

public record CheckoutOrderRequest : IRequest<OperationResult<CheckoutOrderResponse>>
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; }
}
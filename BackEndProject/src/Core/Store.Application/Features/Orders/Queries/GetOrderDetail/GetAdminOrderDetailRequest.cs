using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Orders.Queries;

public record GetAdminOrderDetailRequest : IRequest<OperationResult<GetOrderDetailResponse?>>
{
    [JsonConverter(typeof(OrderEncryptor))]
    public int OrderId { get; init; }
}

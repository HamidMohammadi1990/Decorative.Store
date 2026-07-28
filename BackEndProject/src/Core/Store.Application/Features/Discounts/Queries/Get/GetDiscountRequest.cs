using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Discounts.Queries;

public record GetDiscountRequest : IRequest<OperationResult<GetDiscountResponse?>>
{
    [JsonConverter(typeof(DiscountEncryptor))]
    public int Id { get; init; }
}

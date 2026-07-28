using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Discounts.Commands;

public record DeleteDiscountRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(DiscountEncryptor))]
    public int Id { get; init; }
}

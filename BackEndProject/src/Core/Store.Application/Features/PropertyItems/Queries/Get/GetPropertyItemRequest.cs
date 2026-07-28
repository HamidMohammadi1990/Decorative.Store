using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.PropertyItems.Queries;

public record GetPropertyItemRequest : IRequest<OperationResult<GetPropertyItemResponse?>>
{
    [JsonConverter(typeof(PropertyItemEncryptor))]
    public int Id { get; init; }
}
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.PropertyItems.Commands;

public record DeletePropertyItemRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(PropertyItemEncryptor))]
    public int Id { get; init; }
}
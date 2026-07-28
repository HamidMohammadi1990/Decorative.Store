using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.PropertyItems.Commands;

public record UpdatePropertyItemRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(PropertyItemEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    [JsonConverter(typeof(PropertyEncryptor))]
    public int PropertyId { get; init; }

    public bool Status { get; init; }
    public int Priority { get; init; }
}
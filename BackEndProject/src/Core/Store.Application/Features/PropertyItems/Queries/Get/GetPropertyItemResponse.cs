using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.PropertyItems.Queries;

public record GetPropertyItemResponse
{
    [JsonConverter(typeof(PropertyItemEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    [JsonConverter(typeof(PropertyEncryptor))]
    public int PropertyId { get; init; }

    public int Priority { get; init; }
    public bool IsActive { get; init; }
}
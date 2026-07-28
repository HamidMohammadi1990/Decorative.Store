using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Enums;

namespace Edition.Application.Features.PropertyItems.Queries;

public record GetAllPropertyItemResponse
{
    [JsonConverter(typeof(PropertyItemEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    [JsonConverter(typeof(PropertyEncryptor))]
    public int PropertyId { get; init; }
    public int Priority { get; init; }
    public bool IsActive { get; init; }
    public string PropertyTitle { get; init; } = default!;

    [JsonConverter(typeof(PropertyCategoryEncryptor))]
    public int PropertyCategoryId { get; init; }

    public PropertyType PropertyType { get; init; }
}
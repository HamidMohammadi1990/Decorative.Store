using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Enums;

namespace Edition.Application.Features.Properties.Queries;

public record GetAllPropertyResponse
{
    [JsonConverter(typeof(PropertyEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(PropertyNullableEncryptor))]
    public int? ParentId { get; init; }

    public string Title { get; init; } = default!;

    [JsonConverter(typeof(PropertyCategoryEncryptor))]
    public int PropertyCategoryId { get; init; }

    public string PropertyCategoryTitle { get; init; } = default!;
    public int Priority { get; init; }
    public PropertyType PropertyType { get; init; }
    public bool IsActive { get; init; } = true;
    public string? Description { get; init; }
}
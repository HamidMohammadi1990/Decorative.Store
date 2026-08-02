using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Edition.Application.Features.Localization;
using Store.Domain.Enums;

namespace Edition.Application.Features.Properties.Queries;

public record GetAllPropertyResponse
{
    [JsonConverter(typeof(PropertyEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(PropertyNullableEncryptor))]
    public int? ParentId { get; init; }

    public string Code { get; init; } = default!;

    [JsonConverter(typeof(PropertyCategoryEncryptor))]
    public int PropertyCategoryId { get; init; }

    public string PropertyCategoryCode { get; init; } = default!;
    public int Priority { get; init; }
    public PropertyType PropertyType { get; init; }
    public bool IsActive { get; init; } = true;
    public IReadOnlyList<PropertyTranslationItemResponse> Translations { get; init; } = [];
}

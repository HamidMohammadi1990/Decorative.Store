using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Edition.Application.Features.Localization;
using Store.Domain.Enums;

namespace Edition.Application.Features.PropertyItems.Queries;

public record GetAllPropertyItemResponse
{
    [JsonConverter(typeof(PropertyItemEncryptor))]
    public int Id { get; init; }

    public string Code { get; init; } = default!;

    [JsonConverter(typeof(PropertyEncryptor))]
    public int PropertyId { get; init; }

    public string PropertyCode { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }

    [JsonConverter(typeof(PropertyCategoryEncryptor))]
    public int PropertyCategoryId { get; init; }

    public PropertyType PropertyType { get; init; }
    public IReadOnlyList<TranslationItemResponse> Translations { get; init; } = [];
}

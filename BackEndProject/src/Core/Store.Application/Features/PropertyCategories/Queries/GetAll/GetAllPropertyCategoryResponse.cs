using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Edition.Application.Features.Localization;

namespace Edition.Application.Features.PropertyCategories.Queries;

public record GetAllPropertyCategoryResponse
{
    [JsonConverter(typeof(PropertyCategoryEncryptor))]
    public int Id { get; init; }

    public string Code { get; init; } = default!;
    public bool IsActive { get; init; }
    public IReadOnlyList<TranslationItemResponse> Translations { get; init; } = [];
}

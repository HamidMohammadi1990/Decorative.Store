using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Edition.Application.Features.Localization;

namespace Edition.Application.Features.SubCategories.Queries;

public record GetAllSubCategoryResponse
{
    [JsonConverter(typeof(SubCategoryEncryptor))]
    public int Id { get; init; }

    public string Code { get; init; } = default!;

    [JsonConverter(typeof(CategoryEncryptor))]
    public int CategoryId { get; init; }

    public bool IsActive { get; init; }
    public string CategoryCode { get; init; } = default!;
    public IReadOnlyList<TranslationItemResponse> Translations { get; init; } = [];
    public IReadOnlyList<TranslationItemResponse> CategoryTranslations { get; init; } = [];
}

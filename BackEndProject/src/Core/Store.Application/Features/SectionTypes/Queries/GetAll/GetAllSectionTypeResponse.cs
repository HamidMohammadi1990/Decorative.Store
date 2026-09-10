using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Dtos.Localization;
using System.Text.Json.Serialization;

namespace Edition.Application.Features.SectionTypes.Queries;

public record GetAllSectionTypeResponse
{
    [JsonConverter(typeof(SectionTypeEncryptor))]
    public int Id { get; init; }
    public bool IsActive { get; init; }
    public string? AdminDescription { get; init; }
    public List<SectionTypeTranslationItemDto> Translations { get; init; } = [];
}

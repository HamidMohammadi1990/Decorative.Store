using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Dtos.Localization;
using System.Text.Json.Serialization;

namespace Edition.Application.Features.SectionItems.Queries;

public record GetAllSectionItemResponse
{
    [JsonConverter(typeof(SectionItemEncryptor))]
    public int Id { get; init; }
    [JsonConverter(typeof(SectionEncryptor))]
    public int SectionId { get; init; }
    public int Priority { get; init; }
    public string? Icon { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsActive { get; init; }
    public string SectionTitle { get; init; } = string.Empty;
    public string SectionTypeName { get; init; } = string.Empty;
    public List<SectionItemTranslationItemDto> Translations { get; init; } = [];
}

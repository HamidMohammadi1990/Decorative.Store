using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Dtos.Localization;
using System.Text.Json.Serialization;
using Store.Domain.Enums;

namespace Edition.Application.Features.Pages.Queries;

public record GetAllPageResponse
{
    [JsonConverter(typeof(PageEncryptor))]
    public int Id { get; init; }
    public PageType Type { get; init; }
    public bool IsActive { get; init; }
    public List<PageTranslationItemDto> Translations { get; init; } = [];
}

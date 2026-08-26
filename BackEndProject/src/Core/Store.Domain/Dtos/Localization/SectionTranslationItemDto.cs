namespace Store.Domain.Dtos.Localization;

public record SectionTranslationItemDto
{
    public int LanguageId { get; init; }
    public string Title { get; init; } = default!;
    public string? Description { get; init; }
    public string Url { get; init; } = default!;
}

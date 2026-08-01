namespace Store.Domain.Dtos.Localization;

public record TranslationItemDto
{
    public int LanguageId { get; init; }
    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
}

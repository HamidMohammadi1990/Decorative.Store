namespace Store.Domain.Dtos.Localization;

public record PropertyItemTranslationItemDto
{
    public int LanguageId { get; init; }
    public string Title { get; init; } = default!;
}

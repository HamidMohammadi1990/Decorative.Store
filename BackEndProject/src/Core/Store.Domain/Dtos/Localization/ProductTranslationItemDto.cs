namespace Store.Domain.Dtos.Localization;

public record ProductTranslationItemDto
{
    public int LanguageId { get; init; }
    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string Description { get; init; } = default!;
}

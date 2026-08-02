namespace Store.Domain.Dtos.Localization;

public record PropertyTranslationItemDto
{
    public int LanguageId { get; init; }
    public string Title { get; init; } = default!;
    public string? Description { get; init; }
}

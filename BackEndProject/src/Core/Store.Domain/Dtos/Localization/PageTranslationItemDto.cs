namespace Store.Domain.Dtos.Localization;

public record PageTranslationItemDto
{
    public int LanguageId { get; init; }
    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string? MetaTitle { get; init; }
    public string? MetaDescription { get; init; }
}

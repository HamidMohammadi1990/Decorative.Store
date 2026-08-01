namespace Edition.Application.Features.Localization;

public record TranslationItemResponse
{
    public int LanguageId { get; init; }
    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
}

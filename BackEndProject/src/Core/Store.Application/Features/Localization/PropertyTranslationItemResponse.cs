namespace Edition.Application.Features.Localization;

public record PropertyTranslationItemResponse
{
    public int LanguageId { get; init; }
    public string Title { get; init; } = default!;
    public string? Description { get; init; }
}

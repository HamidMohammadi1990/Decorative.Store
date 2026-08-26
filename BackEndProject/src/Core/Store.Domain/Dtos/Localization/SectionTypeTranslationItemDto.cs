namespace Store.Domain.Dtos.Localization;

public record SectionTypeTranslationItemDto
{
    public int LanguageId { get; init; }
    public string Name { get; init; } = default!;
}

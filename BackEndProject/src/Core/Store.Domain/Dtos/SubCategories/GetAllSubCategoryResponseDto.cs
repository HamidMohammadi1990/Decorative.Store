using Store.Domain.Dtos.Localization;

namespace Store.Domain.Dtos.SubCategories;

public record GetAllSubCategoryResponseDto
{
    public int Id { get; init; }
    public string Code { get; init; } = default!;
    public int CategoryId { get; init; }
    public bool IsActive { get; init; }
    public string CategoryCode { get; init; } = default!;
    public IReadOnlyList<TranslationItemDto> Translations { get; init; } = [];
    public IReadOnlyList<TranslationItemDto> CategoryTranslations { get; init; } = [];
}

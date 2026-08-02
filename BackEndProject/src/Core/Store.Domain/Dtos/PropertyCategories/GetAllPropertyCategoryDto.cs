using Store.Domain.Dtos.Localization;

namespace Store.Domain.Dtos.PropertyCategories;

public record GetAllPropertyCategoryDto
{
    public int Id { get; init; }
    public string Code { get; init; } = default!;
    public bool IsActive { get; init; }
    public IReadOnlyList<PropertyItemTranslationItemDto> Translations { get; init; } = [];
}

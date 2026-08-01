using Store.Domain.Dtos.Localization;

namespace Store.Domain.Dtos.Categories;

public record GetAllCategoryResponseDto
{
    public int Id { get; init; }
    public string Code { get; init; } = default!;
    public bool IsActive { get; init; }
    public IReadOnlyList<TranslationItemDto> Translations { get; init; } = [];
}

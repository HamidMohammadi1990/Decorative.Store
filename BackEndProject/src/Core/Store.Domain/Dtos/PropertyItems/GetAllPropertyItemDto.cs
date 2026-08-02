using Store.Domain.Dtos.Localization;
using Store.Domain.Enums;

namespace Store.Domain.Dtos.PropertyItems;

public record GetAllPropertyItemDto
{
    public int Id { get; init; }
    public string Code { get; init; } = default!;
    public int PropertyId { get; init; }
    public string PropertyCode { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
    public int PropertyCategoryId { get; init; }
    public PropertyType PropertyType { get; init; }
    public IReadOnlyList<PropertyItemTranslationItemDto> Translations { get; init; } = [];
}

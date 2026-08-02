using Store.Domain.Enums;
using Store.Domain.Dtos.Localization;

namespace Store.Domain.Dtos.Properties;

public record GetAllPropertyDto
{
    public int Id { get; init; }
    public int? ParentId { get; init; }
    public string Code { get; init; } = default!;
    public int PropertyCategoryId { get; init; }
    public string PropertyCategoryCode { get; init; } = default!;
    public int Priority { get; init; }
    public PropertyType PropertyType { get; init; }
    public bool IsActive { get; init; } = true;
    public IReadOnlyList<PropertyTranslationItemDto> Translations { get; init; } = [];
}

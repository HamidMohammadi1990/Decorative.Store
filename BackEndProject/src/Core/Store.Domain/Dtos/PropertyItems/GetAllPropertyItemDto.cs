using Store.Domain.Enums;

namespace Store.Domain.Dtos.PropertyItems;

public record GetAllPropertyItemDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public int PropertyId { get; init; }
    public int Priority { get; init; }
    public bool IsActive { get; init; }
    public string PropertyTitle { get; init; } = default!;
    public int PropertyCategoryId { get; init; }
    public PropertyType PropertyType { get; init; }
}
using Store.Domain.Enums;

namespace Store.Domain.Dtos.Properties;

public record GetAllPropertyDto
{
    public int Id { get; init; }
    public int? ParentId { get; init; }
    public string Title { get; init; } = default!;
    public int PropertyCategoryId { get; init; }
    public string PropertyCategoryTitle { get; init; } = default!;
    public int Priority { get; init; }
    public PropertyType PropertyType { get; init; }
    public bool IsActive { get; init; } = true;
    public string? Description { get; init; }
}
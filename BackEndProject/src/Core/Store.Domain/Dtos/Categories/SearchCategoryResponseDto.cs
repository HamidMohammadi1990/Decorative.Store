namespace Store.Domain.Dtos.Categories;

public record SearchCategoryResponseDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string Code { get; init; } = default!;
}
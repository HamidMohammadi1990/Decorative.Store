namespace Edition.Application.Features.SubCategories.Queries;

public record GetSubCategoryResponse
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string Code { get; init; } = default!;
    public int CategoryId { get; init; }
    public bool IsActive { get; init; }
}
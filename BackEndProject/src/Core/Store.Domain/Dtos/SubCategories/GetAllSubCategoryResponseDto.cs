namespace Store.Domain.Dtos.SubCategories;

public record GetAllSubCategoryResponseDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string Code { get; init; } = default!;
    public int CategoryId { get; init; }
    public bool IsActive { get; init; }

    public string CategoryTitle { get; init; } = default!;
    public string CategorySlug { get; init; } = default!;
    public string CategoryCode { get; init; } = default!;
}
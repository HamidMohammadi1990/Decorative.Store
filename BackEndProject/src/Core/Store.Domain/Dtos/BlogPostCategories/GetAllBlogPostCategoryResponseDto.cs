namespace Store.Domain.Dtos.BlogPostCategories;

public record GetAllBlogPostCategoryResponseDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public bool IsActive { get; init; }
}
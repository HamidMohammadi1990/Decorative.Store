namespace Store.Domain.Dtos.BlogPostCategories;

public record SearchBlogPostCategoryResponseDto
{
    public int Id { get; init; }
    public string Code { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public bool IsActive { get; init; }
    public int PostCount { get; init; }
}

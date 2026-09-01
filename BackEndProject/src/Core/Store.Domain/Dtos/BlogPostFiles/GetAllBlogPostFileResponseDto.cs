namespace Store.Domain.Dtos.BlogPostFiles;

public record GetAllBlogPostFileResponseDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public int BlogPostId { get; init; }
    public string BlogPostTitle { get; init; } = default!;
    public string FileName { get; init; } = default!;
    public bool IsActive { get; init; }
    public bool IsMain { get; init; }
}

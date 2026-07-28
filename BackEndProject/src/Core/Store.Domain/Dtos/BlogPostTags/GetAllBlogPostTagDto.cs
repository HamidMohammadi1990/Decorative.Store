namespace Store.Domain.Dtos.BlogPostTags;

public record GetAllBlogPostTagDto
{
    public int Id { get; init; }
    public string TagTitle { get; init; } = default!;
    public int TagId { get; init; }
    public string BlogPostTitle { get; init; } = default!;
    public int BlogPostId { get; init; }
}
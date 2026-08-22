using Store.Domain.Dtos.BlogPostComments;

namespace Store.Domain.Dtos.BlogPosts;

public record BlogPostDetailDto
{
    public SearchBlogPostDto Post { get; init; } = default!;
    public string CategorySlug { get; init; } = string.Empty;
    public List<SearchBlogPostCommentResponseDto> Comments { get; init; } = [];
    public List<BlogPostDetailRelatedDto> RelatedPosts { get; init; } = [];
    public Dictionary<string, string> CategoryLabels { get; init; } = new();
}

public record BlogPostDetailRelatedDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string CategoryTitle { get; init; } = default!;
    public string CategorySlug { get; init; } = default!;
    public int CategoryId { get; init; }
    public string MetaDescription { get; init; } = default!;
    public string SeoKeywords { get; init; } = default!;
    public int UserId { get; init; }
    public int ReadingTimeInMinutes { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? UpdatedOnUtc { get; init; }
    public DateTime? PublishedOnUtc { get; init; }
}

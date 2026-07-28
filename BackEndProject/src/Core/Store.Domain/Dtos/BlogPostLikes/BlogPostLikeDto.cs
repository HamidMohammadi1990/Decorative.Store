namespace Store.Domain.Dtos.BlogPostLikes;

public record BlogPostLikeDto
{
    public int Id { get; init; }
    public string? UserName { get; init; }
    public int BlogPostId { get; init; }
    public string BlogPostTitle { get; init; } = default!;
    public DateTime CreatedOnUtc { get; init; }
    public string ClientIP { get; init; } = default!;
}
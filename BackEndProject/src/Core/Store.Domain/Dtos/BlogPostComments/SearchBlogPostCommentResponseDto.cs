namespace Store.Domain.Dtos.BlogPostComments;

public record SearchBlogPostCommentResponseDto
{
    public int Id { get; init; }
    public int? ParentId { get; init; }
    public string Content { get; init; } = default!;
    public int CreatedByUserId { get; init; }
    public string? CreatedByUserFirstName { get; init; }
    public string? CreatedByUserLastName { get; init; }
    public int BlogPostId { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ApprovedOnUtc { get; init; }
}
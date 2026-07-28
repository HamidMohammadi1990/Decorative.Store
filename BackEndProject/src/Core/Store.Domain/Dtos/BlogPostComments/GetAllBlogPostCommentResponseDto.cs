namespace Store.Domain.Dtos.BlogPostComments;

public record GetAllBlogPostCommentResponseDto
{
    public int Id { get; init; }
    public int? ParentId { get; init; }
    public string Content { get; init; } = default!;
    public int CreatedByUserId { get; init; }
    public string? CreatedByUserFirstName { get; init; }
    public string? CreatedByUserLastName { get; init; }
    public int? ApprovedByUserId { get; init; }
    public string? ApprovedByUserFirstName { get; init; }
    public string? ApprovedByUserLastName { get; init; }
    public int BlogPostId { get; init; }
    public string BlogPostTitle { get; init; } = default!;
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ApprovedOnUtc { get; init; }
    public bool IsApproved { get; init; }
}
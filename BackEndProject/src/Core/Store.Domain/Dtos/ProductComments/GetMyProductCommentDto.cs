namespace Store.Domain.Dtos.ProductComments;

public record GetMyProductCommentDto
{
    public int Id { get; init; }
    public int ProductId { get; init; }
    public string ProductTitle { get; init; } = default!;
    public string ProductSlug { get; init; } = default!;
    public int CommentRate { get; init; }
    public string Description { get; init; } = default!;
    public bool IsActive { get; init; }
}

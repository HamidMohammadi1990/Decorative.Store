namespace Store.Domain.Dtos.ProductComments;

public record SearchProductCommentResponseDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string? UserFirstName { get; init; }
    public string? UserLastName { get; init; }
    public string UserName { get; init; } = default!;
    public int CommentTopicId { get; init; }
    public string CommentTopicTitle { get; init; } = default!;
    public int CommentRate { get; init; }
    public string Description { get; init; } = default!;
    public int QualityRating { get; init; }
    public int AffordableRating { get; init; }
    public int ProductId { get; init; }
    public string ProductTitle { get; init; } = default!;
    public int CompanyId { get; init; }
    public string CompanyName { get; init; } = default!;    
}
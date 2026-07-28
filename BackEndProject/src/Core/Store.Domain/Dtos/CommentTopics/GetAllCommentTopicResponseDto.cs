namespace Store.Domain.Dtos.CommentTopics;

public record GetAllCommentTopicResponseDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
}
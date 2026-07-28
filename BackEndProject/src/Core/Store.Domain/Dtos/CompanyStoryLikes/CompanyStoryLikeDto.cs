namespace Store.Domain.Dtos.CompanyStoryLikes;

public record CompanyStoryLikeDto
{
    public int Id { get; init; }
    public string? UserName { get; init; }
    public int CompanyStoryId { get; init; }
    public string? CompanyStoryCaption { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public string ClientIP { get; init; } = default!;
}

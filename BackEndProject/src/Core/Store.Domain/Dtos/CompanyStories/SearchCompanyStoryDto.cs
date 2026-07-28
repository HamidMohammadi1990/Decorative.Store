namespace Store.Domain.Dtos.CompanyStories;

public record SearchCompanyStoryDto
{
    public int Id { get; init; }
    public int CompanyId { get; init; }
    public string CompanyName { get; init; } = default!;
    public int CreatedByUserId { get; init; }
    public string CreatedByUserFirstName { get; init; } = default!;
    public string CreatedByUserLastName { get; init; } = default!;
    public string? Caption { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ExpiresAtUtc { get; init; }
    public int LikeCount { get; init; }
    public int CommentCount { get; init; }
    public List<CompanyStoryItemDto> Items { get; init; } = [];
}

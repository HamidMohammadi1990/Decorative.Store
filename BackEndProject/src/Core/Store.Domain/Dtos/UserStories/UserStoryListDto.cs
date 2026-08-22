using Store.Domain.Enums;

namespace Store.Domain.Dtos.UserStories;

public record UserStoryListDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string Title { get; init; } = default!;
    public string? Caption { get; init; }
    public StoryMediaType MediaType { get; init; }
    public string MediaPath { get; init; } = default!;
    public string MediaAlt { get; init; } = default!;
    public string? PosterPath { get; init; }
    public string? ProductSlug { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public string? OwnerFirstName { get; init; }
    public string? OwnerLastName { get; init; }
}

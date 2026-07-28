using Store.Domain.Enums;

namespace Store.Domain.Dtos.CompanyStories;

public record CompanyStoryItemDto
{
    public int Id { get; init; }
    public CompanyStoryMediaType MediaType { get; init; }
    public string FileName { get; init; } = default!;
    public int Priority { get; init; }
    public int? DurationSeconds { get; init; }
}

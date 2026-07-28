using Store.Domain.Enums;

namespace Edition.Application.Features.CompanyStories;

public record CompanyStoryItemResponse
{
    public int Id { get; init; }
    public CompanyStoryMediaType MediaType { get; init; }
    public string FileName { get; init; } = default!;
    public int Priority { get; init; }
    public int? DurationSeconds { get; init; }
}

public record CompanyStoryItemInput
{
    public CompanyStoryMediaType MediaType { get; init; }
    public string FileName { get; init; } = default!;
    public int Priority { get; init; }
    public int? DurationSeconds { get; init; }
}

using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using MediatR;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.UserStories.Queries;

public record GetMyUserStoriesRequest : IRequest<OperationResult<GetMyUserStoriesResponse>>;

public record GetMyUserStoriesResponse
{
    public List<GetMyUserStoryItemResponse> Items { get; init; } = [];
}

public record GetMyUserStoryItemResponse
{
    [JsonConverter(typeof(UserStoryEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public string? Caption { get; init; }
    public StoryMediaType MediaType { get; init; }
    public string MediaUrl { get; init; } = default!;
    public string MediaAlt { get; init; } = default!;
    public string? PosterUrl { get; init; }
    public string? ProductSlug { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedOnUtc { get; init; }
}

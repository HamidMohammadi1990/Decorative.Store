using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using MediatR;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.UserStories.Queries;

public record SearchActiveUserStoriesRequest : IRequest<OperationResult<SearchActiveUserStoriesResponse>>
{
    public int Limit { get; init; } = 20;
}

public record SearchActiveUserStoriesResponse
{
    public List<SearchActiveUserStoryItemResponse> Items { get; init; } = [];
}

public record SearchActiveUserStoryItemResponse
{
    [JsonConverter(typeof(UserStoryEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    public string Title { get; init; } = default!;
    public string? Caption { get; init; }
    public StoryMediaType MediaType { get; init; }
    public string MediaUrl { get; init; } = default!;
    public string MediaAlt { get; init; } = default!;
    public string? PosterUrl { get; init; }
    public string? ProductSlug { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public string? OwnerFirstName { get; init; }
    public string? OwnerLastName { get; init; }
    public int LikeCount { get; init; }
    public int CommentCount { get; init; }
    public bool IsLikedByCurrentUser { get; init; }
}

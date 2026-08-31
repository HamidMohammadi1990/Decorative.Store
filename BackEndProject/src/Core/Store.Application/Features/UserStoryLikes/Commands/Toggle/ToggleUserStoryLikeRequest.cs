using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.UserStoryLikes.Commands;

public record ToggleUserStoryLikeRequest : IRequest<OperationResult<ToggleUserStoryLikeResponse>>
{
    [JsonConverter(typeof(UserStoryEncryptor))]
    public int UserStoryId { get; init; }
}

public record ToggleUserStoryLikeResponse
{
    public bool Liked { get; init; }
    public int LikeCount { get; init; }
}

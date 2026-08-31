using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.UserStoryComments.Commands;

public record CreateUserStoryCommentRequest : IRequest<OperationResult<CreateUserStoryCommentResponse>>
{
    [JsonConverter(typeof(UserStoryEncryptor))]
    public int UserStoryId { get; init; }

    public string Content { get; init; } = default!;
}

public record CreateUserStoryCommentResponse
{
    [JsonConverter(typeof(UserStoryCommentEncryptor))]
    public int Id { get; init; }
}

using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.UserStoryComments.Commands;

public record ApproveUserStoryCommentRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(UserStoryCommentEncryptor))]
    public int Id { get; init; }
}

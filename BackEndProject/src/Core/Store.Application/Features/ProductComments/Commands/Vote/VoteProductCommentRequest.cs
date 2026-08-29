using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductComments.Commands;

public record VoteProductCommentRequest : IRequest<OperationResult<VoteProductCommentResponse>>
{
    [JsonConverter(typeof(ProductCommentEncryptor))]
    public int Id { get; init; }

    public bool IsHelpful { get; init; }
}

public record VoteProductCommentResponse
{
    public int HelpfulCount { get; init; }
    public int NotHelpfulCount { get; init; }
    public bool? UserVoteHelpful { get; init; }
}

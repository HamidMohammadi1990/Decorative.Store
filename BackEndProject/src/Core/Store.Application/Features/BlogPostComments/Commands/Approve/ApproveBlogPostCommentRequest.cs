using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.BlogPostComments.Commands;

public record ApproveBlogPostCommentRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(BlogPostCommentEncryptor))]
    public int Id { get; init; }
}
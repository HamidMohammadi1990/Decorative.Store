using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.BlogPostComments.Queries;

public record GetBlogPostCommentRequest : IRequest<OperationResult<GetBlogPostCommentResponse?>>
{
    [JsonConverter(typeof(BlogPostCommentEncryptor))]
    public int Id { get; init; }
}
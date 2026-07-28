using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.BlogPostComments.Commands;

public record CreateBlogPostCommentResponse
{
    [JsonConverter(typeof(BlogPostCommentEncryptor))]
    public int Id { get; init; }
}
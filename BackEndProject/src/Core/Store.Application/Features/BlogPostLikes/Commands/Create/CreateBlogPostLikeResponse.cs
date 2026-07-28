using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.BlogPostLikes.Commands;

public record CreateBlogPostLikeResponse
{
    [JsonConverter(typeof(BlogPostLikeEncryptor))]
    public int Id { get; init; }
}
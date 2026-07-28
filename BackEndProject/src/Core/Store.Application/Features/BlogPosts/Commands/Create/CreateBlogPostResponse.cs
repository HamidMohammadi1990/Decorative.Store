using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.BlogPosts.Commands;

public record CreateBlogPostResponse
{
    [JsonConverter(typeof(BlogPostEncryptor))]
    public int Id { get; init; }
}
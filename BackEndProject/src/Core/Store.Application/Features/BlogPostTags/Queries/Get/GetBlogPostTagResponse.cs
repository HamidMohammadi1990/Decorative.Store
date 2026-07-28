using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.BlogPostTags.Queries;

public record GetBlogPostTagResponse
{
    [JsonConverter(typeof(BlogPostTagEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(TagEncryptor))]
    public int TagId { get; init; }

    [JsonConverter(typeof(BlogPostEncryptor))]
    public int BlogPostId { get; init; }
}
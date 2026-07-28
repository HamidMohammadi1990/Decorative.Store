using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.BlogPostTags.Queries;

public record GetAllBlogPostTagResponse
{
    [JsonConverter(typeof(BlogPostTagEncryptor))]
    public int Id { get; init; }

    public string TagTitle { get; init; } = default!;

    [JsonConverter(typeof(TagEncryptor))]
    public int TagId { get; init; }

    public string BlogPostTitle { get; init; } = default!;

    [JsonConverter(typeof(BlogPostEncryptor))]
    public int BlogPostId { get; init; }
}
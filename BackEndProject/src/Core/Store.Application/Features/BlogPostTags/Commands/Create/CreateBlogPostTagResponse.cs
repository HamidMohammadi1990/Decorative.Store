using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.BlogPostTags.Commands;

public record CreateBlogPostTagResponse
{
    [JsonConverter(typeof(BlogPostTagEncryptor))]
    public int Id { get; init; }
}
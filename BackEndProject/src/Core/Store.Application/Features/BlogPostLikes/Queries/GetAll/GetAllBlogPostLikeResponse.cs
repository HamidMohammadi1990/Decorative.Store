using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.BlogPostLikes.Queries;

public record GetAllBlogPostLikeResponse
{
    [JsonConverter(typeof(BlogPostLikeEncryptor))]
    public int Id { get; init; }
    public string? UserName { get; init; }

    [JsonConverter(typeof(BlogPostEncryptor))]
    public int BlogPostId { get; init; }
    public string BlogPostTitle { get; init; } = default!;
    public DateTime CreatedOnUtc { get; init; }
    public string ClientIP { get; init; } = default!;
}
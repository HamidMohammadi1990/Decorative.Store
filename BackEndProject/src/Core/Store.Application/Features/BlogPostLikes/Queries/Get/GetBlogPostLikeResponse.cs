using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.BlogPostLikes.Queries;

public record GetBlogPostLikeResponse
{
    [JsonConverter(typeof(BlogPostLikeEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int? UserId { get; init; }

    [JsonConverter(typeof(BlogPostEncryptor))]
    public int BlogPostId { get; init; }

    public DateTime CreatedOnUtc { get; init; }
    public string ClientIP { get; init; } = default!;
}
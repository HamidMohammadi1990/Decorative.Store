using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.BlogPosts.Queries;

public record GetBlogPostResponse
{
    [JsonConverter(typeof(BlogPostEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;

    [JsonConverter(typeof(BlogPostCategoryEncryptor))]
    public int BlogPostCategoryId { get; init; }

    public string MetaDescription { get; init; } = default!;
    public string SeoKeywords { get; init; } = default!;
    public string Content { get; init; } = default!;

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    public int ReadingTimeInMinutes { get; set; }
    public DateTime CreatedOnUtc { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedOnUtc { get; init; }
    public DateTime? PublishedOnUtc { get; init; }
    public bool IsActive { get; init; }
    public bool IsPublished { get; init; }
}
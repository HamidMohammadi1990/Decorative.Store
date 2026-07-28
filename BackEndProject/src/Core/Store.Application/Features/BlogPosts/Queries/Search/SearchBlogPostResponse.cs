using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.BlogPosts.Queries;

public record SearchBlogPostResponse
{
    [JsonConverter(typeof(BlogPostEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string CategoryTitle { get; set; } = default!;


    [JsonConverter(typeof(CategoryEncryptor))]
    public int CategoryId { get; init; }

    public string MetaDescription { get; init; } = default!;
    public string SeoKeywords { get; init; } = default!;
    public string Content { get; init; } = default!;
    public string UserFirstName { get; set; } = default!;
    public string UserLastName { get; set; } = default!;

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    public int ReadingTimeInMinutes { get; set; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? UpdatedOnUtc { get; init; }
    public DateTime? PublishedOnUtc { get; init; }
}
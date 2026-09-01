using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.BlogPosts.Queries;

public record GetBlogPostDetailResponse
{
    public bool NotFound { get; init; }
    public BlogPostDetailPostResponse? Post { get; init; }
    public List<BlogPostDetailCommentResponse> Comments { get; init; } = [];
    public List<BlogPostDetailRelatedResponse> RelatedPosts { get; init; } = [];
    public Dictionary<string, string> CategoryLabels { get; init; } = new();
}

public record BlogPostDetailPostResponse
{
    [JsonConverter(typeof(BlogPostEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string CategoryTitle { get; init; } = string.Empty;
    public string CategorySlug { get; init; } = string.Empty;

    [JsonConverter(typeof(BlogPostCategoryEncryptor))]
    public int CategoryId { get; init; }

    public string MetaDescription { get; init; } = string.Empty;
    public string SeoKeywords { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public string UserFirstName { get; init; } = string.Empty;
    public string UserLastName { get; init; } = string.Empty;

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    public int ReadingTimeInMinutes { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? UpdatedOnUtc { get; init; }
    public DateTime? PublishedOnUtc { get; init; }
    public int CommentCount { get; init; }
    public int LikeCount { get; init; }
    public bool IsFeatured { get; init; }
    public List<string> TagTitles { get; init; } = [];
    public List<BlogPostDetailImageResponse> Images { get; init; } = [];
}

public record BlogPostDetailImageResponse
{
    public string Title { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public bool IsMain { get; init; }
}

public record BlogPostDetailCommentResponse
{
    [JsonConverter(typeof(BlogPostCommentEncryptor))]
    public int Id { get; init; }

    public string Content { get; init; } = string.Empty;
    public string CreatedByUserFirstName { get; init; } = string.Empty;
    public string CreatedByUserLastName { get; init; } = string.Empty;
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ApprovedOnUtc { get; init; }
}

public record BlogPostDetailRelatedResponse
{
    [JsonConverter(typeof(BlogPostEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string CategoryTitle { get; init; } = string.Empty;
    public string CategorySlug { get; init; } = string.Empty;

    [JsonConverter(typeof(BlogPostCategoryEncryptor))]
    public int CategoryId { get; init; }

    public string MetaDescription { get; init; } = string.Empty;
    public string SeoKeywords { get; init; } = string.Empty;
    public int ReadingTimeInMinutes { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? UpdatedOnUtc { get; init; }
    public DateTime? PublishedOnUtc { get; init; }
}

using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.BlogPosts.Commands;

public record UpdateBlogPostRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(BlogPostEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(BlogPostCategoryEncryptor))]
    public int CategoryId { get; init; }

    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string MetaDescription { get; init; } = default!;
    public string SeoKeywords { get; init; } = default!;
    public string Content { get; init; } = default!;
    public int ReadingTimeInMinutes { get; set; }
}
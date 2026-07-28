using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.BlogPostCategories.Queries;

public record SearchBlogPostCategoryResponse
{
    [JsonConverter(typeof(BlogPostCategoryEncryptor))]
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public bool IsActive { get; init; }
}
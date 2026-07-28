using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.BlogPostCategories.Commands;

public record CreateBlogPostCategoryResponse
{
    [JsonConverter(typeof(BlogPostCategoryEncryptor))]
    public int Id { get; init; }
}
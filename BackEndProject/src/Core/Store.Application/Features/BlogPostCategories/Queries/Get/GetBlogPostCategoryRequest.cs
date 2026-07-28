using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.BlogPostCategories.Queries;

public record GetBlogPostCategoryRequest : IRequest<OperationResult<GetBlogPostCategoryResponse?>>
{
    [JsonConverter(typeof(BlogPostCategoryEncryptor))]
    public int Id { get; init; }
}
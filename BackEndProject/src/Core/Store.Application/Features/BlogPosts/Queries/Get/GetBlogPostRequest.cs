using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.BlogPosts.Queries;

public record GetBlogPostRequest : IRequest<OperationResult<GetBlogPostResponse?>>
{
    [JsonConverter(typeof(BlogPostEncryptor))]
    public int Id { get; init; }
}
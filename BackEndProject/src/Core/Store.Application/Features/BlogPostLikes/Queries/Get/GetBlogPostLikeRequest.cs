using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.BlogPostLikes.Queries;

public record GetBlogPostLikeRequest : IRequest<OperationResult<GetBlogPostLikeResponse?>>
{
    [JsonConverter(typeof(BlogPostLikeEncryptor))]
    public int Id { get; init; }
}
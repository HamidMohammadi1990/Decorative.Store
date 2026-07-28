using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.BlogPostLikes.Commands;

public record CreateBlogPostLikeRequest : IRequest<OperationResult<CreateBlogPostLikeResponse>>
{    
    [JsonConverter(typeof(BlogPostEncryptor))]
    public int BlogPostId { get; init; }
}
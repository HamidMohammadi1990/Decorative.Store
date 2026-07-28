using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.BlogPostTags.Commands;

public record CreateBlogPostTagRequest : IRequest<OperationResult<CreateBlogPostTagResponse>>
{
    [JsonConverter(typeof(TagEncryptor))]
    public int TagId { get; init; }

    [JsonConverter(typeof(BlogPostEncryptor))]
    public int BlogPostId { get; init; }
}
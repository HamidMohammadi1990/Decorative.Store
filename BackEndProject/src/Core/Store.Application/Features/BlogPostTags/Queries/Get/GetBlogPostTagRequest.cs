using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.BlogPostTags.Queries;

public record GetBlogPostTagRequest : IRequest<OperationResult<GetBlogPostTagResponse?>>
{
    [JsonConverter(typeof(BlogPostTagEncryptor))]
    public int Id { get; init; }
}
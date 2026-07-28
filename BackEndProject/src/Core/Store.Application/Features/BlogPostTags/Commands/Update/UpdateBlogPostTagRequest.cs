using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.BlogPostTags.Commands;

public record UpdateBlogPostTagRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(BlogPostTagEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(TagEncryptor))]
    public int TagId { get; init; }

    [JsonConverter(typeof(BlogPostEncryptor))]
    public int BlogPostId { get; init; }
}
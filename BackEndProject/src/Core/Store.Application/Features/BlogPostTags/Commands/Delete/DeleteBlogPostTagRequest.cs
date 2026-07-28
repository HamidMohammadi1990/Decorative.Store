using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.BlogPostTags.Commands;

public record DeleteBlogPostTagRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(BlogPostTagEncryptor))]
    public int Id { get; init; }
}
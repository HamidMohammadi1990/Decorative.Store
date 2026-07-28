using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.BlogPosts.Commands;

public record PublishBlogPostRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(BlogPostEncryptor))]
    public int Id { get; init; }
}
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.BlogPostComments.Commands;

public record UpdateBlogPostCommentRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(BlogPostCommentEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(BlogPostCommentEncryptor))]
    public int? ParentId { get; init; }

    [JsonConverter(typeof(BlogPostEncryptor))]
    public int BlogPostId { get; init; }

    public string Content { get; init; } = default!;    
}
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.BlogPostComments.Commands;

public record CreateBlogPostCommentRequest : IRequest<OperationResult<CreateBlogPostCommentResponse>>
{
    [JsonConverter(typeof(BlogPostCommentNullableEncryptor))]
    public int? ParentId { get; init; }

    [JsonConverter(typeof(BlogPostEncryptor))]
    public int BlogPostId { get; init; }

    public string Content { get; init; } = default!;    
}
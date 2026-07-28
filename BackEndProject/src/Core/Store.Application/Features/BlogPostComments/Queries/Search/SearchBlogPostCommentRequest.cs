using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.BlogPostComments.Queries;

public record SearchBlogPostCommentRequest : ContentPolicyRequest<BlogPostComment>, IRequest<OperationResult<PagedResult<SearchBlogPostCommentResponse>>>
{
    [JsonConverter(typeof(BlogPostEncryptor))]
    public int BlogPostId { get; set; }

    public PagedRequest Pagination { get; init; } = default!;
}
using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.BlogPostComments.Queries;

public record GetAllBlogPostCommentRequest : ContentPolicyRequest<BlogPostComment>, IRequest<OperationResult<PagedResult<GetAllBlogPostCommentResponse>>>
{
    [JsonConverter(typeof(BlogPostNullableEncryptor))]
    public int? BlogPostId { get; set; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? CreatedByUserId { get; private set; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? ApprovedByUserId { get; private set; }

    public bool? IsApproved { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}
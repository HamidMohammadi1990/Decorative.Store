using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.BlogPostLikes.Queries;

public record GetAllBlogPostLikeRequest : ContentPolicyRequest<BlogPostLike>, IRequest<OperationResult<PagedResult<GetAllBlogPostLikeResponse>>>
{
    [JsonConverter(typeof(BlogPostNullableEncryptor))]
    public int? BlogPostId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}
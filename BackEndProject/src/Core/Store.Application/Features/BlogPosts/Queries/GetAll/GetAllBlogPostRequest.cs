using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.BlogPosts.Queries;

public record GetAllBlogPostRequest : ContentPolicyRequest<BlogPost>, IRequest<OperationResult<PagedResult<GetAllBlogPostResponse>>>
{
    public string? Title { get; init; }
    public string? Slug { get; init; }

    [JsonConverter(typeof(CategoryNullableEncryptor))]
    public int? CategoryId { get; init; }

    public bool? IsActive { get; init; }
    public bool? IsPublished { get; init; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? UserId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}
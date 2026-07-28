using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.BlogPosts.Queries;

public record SearchBlogPostRequest : ContentPolicyRequest<BlogPost>, IRequest<OperationResult<PagedResult<SearchBlogPostResponse>>>
{
    public string? Title { get; init; }
    public string? Slug { get; init; }

    [JsonConverter(typeof(CategoryNullableEncryptor))]
    public int? CategoryId { get; init; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? UserId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}
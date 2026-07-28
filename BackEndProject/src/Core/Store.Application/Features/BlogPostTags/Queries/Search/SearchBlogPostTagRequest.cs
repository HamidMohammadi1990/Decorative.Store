using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.BlogPostTags.Queries;

public record SearchBlogPostTagRequest : ContentPolicyRequest<BlogPostTag>, IRequest<OperationResult<PagedResult<SearchBlogPostTagResponse>>>
{
    [JsonConverter(typeof(BlogPostNullableEncryptor))]
    public int? BlogPostId { get; init; }

    [JsonConverter(typeof(TagNullableEncryptor))]
    public int? TagId { get; init; }

    public string? TagTitle { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
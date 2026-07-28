using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.BlogPostTags.Queries;

public record GetAllBlogPostTagRequest : ContentPolicyRequest<BlogPostTag>, IRequest<OperationResult<PagedResult<GetAllBlogPostTagResponse>>>
{
    [JsonConverter(typeof(BlogPostNullableEncryptor))]
    public int? BlogPostId { get; set; }

    [JsonConverter(typeof(TagNullableEncryptor))]
    public int? TagId { get; set; }

    public string? TagTitle { get; set; }
    public PagedRequest Pagination { get; init; } = default!;
}
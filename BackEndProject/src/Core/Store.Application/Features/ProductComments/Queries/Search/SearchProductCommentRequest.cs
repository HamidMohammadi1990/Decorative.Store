using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductComments.Queries;

public record SearchProductCommentRequest : ContentPolicyRequest<ProductComment>, IRequest<OperationResult<PagedResult<SearchProductCommentResponse>>>
{
    [JsonConverter(typeof(ProductNullableEncryptor))]
    public int? ProductId { get; init; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? UserId { get; init; }

    [JsonConverter(typeof(CommentTopicNullableEncryptor))]
    public int? CommentTopicId { get; init; }    

    public PagedRequest Pagination { get; init; } = default!;
}
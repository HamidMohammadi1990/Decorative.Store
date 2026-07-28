using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductProperties.Queries;

public record SearchProductPropertyRequest : ContentPolicyRequest<ProductProperty>, IRequest<OperationResult<PagedResult<SearchProductPropertyResponse>>>
{
    [JsonConverter(typeof(ProductNullableEncryptor))]
    public int? ProductId { get; init; }
    [JsonConverter(typeof(PropertyNullableEncryptor))]
    public int? PropertyId { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}

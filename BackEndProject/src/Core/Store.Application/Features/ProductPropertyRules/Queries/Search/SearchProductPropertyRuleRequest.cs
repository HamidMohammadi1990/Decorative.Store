using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductPropertyRules.Queries;

public record SearchProductPropertyRuleRequest : ContentPolicyRequest<ProductPropertyRule>, IRequest<OperationResult<PagedResult<SearchProductPropertyRuleResponse>>>
{

    [JsonConverter(typeof(ProductPropertyNullableEncryptor))]
    public int? ProductPropertyId { get; init; }

    public PropertyType? PropertyType { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}

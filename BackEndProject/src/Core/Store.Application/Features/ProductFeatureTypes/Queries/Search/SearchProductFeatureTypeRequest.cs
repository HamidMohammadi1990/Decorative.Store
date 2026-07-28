using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductFeatureTypes.Queries;

public record SearchProductFeatureTypeRequest : ContentPolicyRequest<ProductFeatureType>, IRequest<OperationResult<PagedResult<SearchProductFeatureTypeResponse>>>
{
    public string? Name { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}

using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductFeatureTypes.Queries;

public record GetAllProductFeatureTypeRequest : ContentPolicyRequest<ProductFeatureType>, IRequest<OperationResult<PagedResult<GetAllProductFeatureTypeResponse>>>
{
    public string? Name { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}

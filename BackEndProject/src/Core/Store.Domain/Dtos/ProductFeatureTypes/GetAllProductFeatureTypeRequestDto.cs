using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.ProductFeatureTypes;

public record GetAllProductFeatureTypeRequestDto : IContentPolicyQueryDto<ProductFeatureType>
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Name { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<ProductFeatureType, bool>>? ContentFilter { get; set; }
}
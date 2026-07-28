using System.Linq.Expressions;
using Edition.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.ProductProperties;

public record GetAllProductPropertyRequestDto : IContentPolicyQueryDto<ProductProperty>
{
    [QueryFilter]
    public int? ProductId { get; init; }

    [QueryFilter]
    public int? PropertyId { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<ProductProperty, bool>>? ContentFilter { get; set; }
}

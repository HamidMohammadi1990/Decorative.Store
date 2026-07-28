using System.Linq.Expressions;
using Edition.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.ProductDescriptions;

public record SearchProductDescriptionRequestDto : IContentPolicyQueryDto<ProductDescription>
{
    [QueryFilter]
    public int ProductId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<ProductDescription, bool>>? ContentFilter { get; set; }
}

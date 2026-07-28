using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.ProductFiles;

public record SearchProductFileRequestDto : IContentPolicyQueryDto<ProductFile>
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter]
    public int? ProductId { get; init; }

    [QueryFilter]
    public bool? IsMain { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<ProductFile, bool>>? ContentFilter { get; set; }
}

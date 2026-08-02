using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.FinancialYears;

public record GetAllFinancialYearRequestDto : IContentPolicyQueryDto<FinancialYear>
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Name { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; } = true;

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<FinancialYear, bool>>? ContentFilter { get; set; }
}

using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Banks;

public record GetAllBankRequestDto : IContentPolicyQueryDto<Bank>
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<Bank, bool>>? ContentFilter { get; set; }
}

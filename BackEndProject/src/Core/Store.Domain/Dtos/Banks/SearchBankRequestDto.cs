using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Banks;

public record SearchBankRequestDto : IContentPolicyQueryDto<Bank>
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<Bank, bool>>? ContentFilter { get; set; }
}

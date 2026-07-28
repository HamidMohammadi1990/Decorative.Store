using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.SectionTypes;

public record GetAllSectionTypeRequestDto : IContentPolicyQueryDto<SectionType>
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Name { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<SectionType, bool>>? ContentFilter { get; set; }
}

public record SearchSectionTypeRequestDto : IContentPolicyQueryDto<SectionType>
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Name { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<SectionType, bool>>? ContentFilter { get; set; }
}

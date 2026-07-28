using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.SectionItems;

public record GetAllSectionItemRequestDto : IContentPolicyQueryDto<SectionItem>
{
    [QueryFilter]
    public int? SectionId { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<SectionItem, bool>>? ContentFilter { get; set; }
}

public record SearchSectionItemRequestDto : IContentPolicyQueryDto<SectionItem>
{
    [QueryFilter]
    public int? SectionId { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<SectionItem, bool>>? ContentFilter { get; set; }
}

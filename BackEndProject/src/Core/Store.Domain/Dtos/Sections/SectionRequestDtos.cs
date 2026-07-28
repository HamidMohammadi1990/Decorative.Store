using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Sections;

public record GetAllSectionRequestDto : IContentPolicyQueryDto<Section>
{
    [QueryFilter]
    public int? SectionTypeId { get; init; }

    [QueryFilter]
    public int? ParentId { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Url { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<Section, bool>>? ContentFilter { get; set; }
}

public record SearchSectionRequestDto : IContentPolicyQueryDto<Section>
{
    [QueryFilter]
    public int? SectionTypeId { get; init; }

    [QueryFilter]
    public int? ParentId { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Url { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<Section, bool>>? ContentFilter { get; set; }
}

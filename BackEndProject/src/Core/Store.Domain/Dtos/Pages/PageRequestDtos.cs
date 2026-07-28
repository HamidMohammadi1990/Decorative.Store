using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Pages;

public record GetAllPageRequestDto : IContentPolicyQueryDto<Page>
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Slug { get; init; }

    [QueryFilter]
    public PageType? Type { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<Page, bool>>? ContentFilter { get; set; }
}

public record SearchPageRequestDto : IContentPolicyQueryDto<Page>
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Slug { get; init; }

    [QueryFilter]
    public PageType? Type { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<Page, bool>>? ContentFilter { get; set; }
}

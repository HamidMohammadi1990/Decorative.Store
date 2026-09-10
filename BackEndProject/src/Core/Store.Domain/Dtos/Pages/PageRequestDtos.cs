using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Localization;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Pages;
public record GetAllPageResponseDto
{
    public int Id { get; init; }
    public PageType Type { get; init; }
    public bool IsActive { get; init; }
    public string? AdminDescription { get; init; }
    public List<PageTranslationItemDto> Translations { get; init; } = [];
}

public record SearchPageResponseDto
{
    public int Id { get; init; }
    public string Slug { get; init; } = default!;
    public string Title { get; init; } = default!;
    public PageType Type { get; init; }
    public string? MetaTitle { get; init; }
    public string? MetaDescription { get; init; }
}


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

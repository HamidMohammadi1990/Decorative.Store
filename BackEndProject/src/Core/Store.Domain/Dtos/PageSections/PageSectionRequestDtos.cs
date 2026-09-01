using System.Linq.Expressions;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.PageSections;

public record GetAllPageSectionResponseDto
{
    public int Id { get; init; }
    public int PageId { get; init; }
    public int SectionId { get; init; }
    public int Priority { get; init; }
    public string PageTitle { get; init; } = string.Empty;
    public string PageSlug { get; init; } = string.Empty;
    public string SectionTitle { get; init; } = string.Empty;
    public string SectionTypeName { get; init; } = string.Empty;
}

public record GetAllPageSectionRequestDto : IContentPolicyQueryDto<PageSection>
{
    [QueryFilter]
    public int? PageId { get; init; }

    [QueryFilter]
    public int? SectionId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<PageSection, bool>>? ContentFilter { get; set; }
}

public record SearchPageSectionRequestDto : IContentPolicyQueryDto<PageSection>
{
    [QueryFilter]
    public int? PageId { get; init; }

    [QueryFilter]
    public int? SectionId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<PageSection, bool>>? ContentFilter { get; set; }
}

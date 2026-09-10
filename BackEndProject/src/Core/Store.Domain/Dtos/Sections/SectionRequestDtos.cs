using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Localization;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Sections;

public record GetAllSectionResponseDto
{
    public int Id { get; init; }
    public int SectionTypeId { get; init; }
    public int? ParentId { get; init; }
    public string? ImageUrl { get; init; }
    public DateTime? StartDateOnUtc { get; init; }
    public DateTime? EndDateOnUtc { get; init; }
    public bool IsActive { get; init; }
    public string? AdminDescription { get; init; }
    public string SectionTypeName { get; init; } = string.Empty;
    public string? ParentTitle { get; init; }
    public List<SectionTranslationItemDto> Translations { get; init; } = [];
}

public record SearchSectionResponseDto
{
    public int Id { get; init; }
    public int SectionTypeId { get; init; }
    public int? ParentId { get; init; }
    public string Title { get; init; } = default!;
    public string? Description { get; init; }
    public string Url { get; init; } = default!;
    public string? ImageUrl { get; init; }
    public bool IsActive { get; init; }
}

public record GetAllSectionRequestDto : IContentPolicyQueryDto<Section>
{
    [QueryFilter]
    public int? SectionTypeId { get; init; }

    [QueryFilter]
    public int? ParentId { get; init; }

    public string? Title { get; init; }

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

    public string? Title { get; init; }

    public string? Url { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<Section, bool>>? ContentFilter { get; set; }
}

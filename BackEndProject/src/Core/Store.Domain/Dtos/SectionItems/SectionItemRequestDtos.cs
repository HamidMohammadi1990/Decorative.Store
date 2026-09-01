using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Localization;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.SectionItems;

public record GetAllSectionItemResponseDto
{
    public int Id { get; init; }
    public int SectionId { get; init; }
    public int Priority { get; init; }
    public string? Icon { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsActive { get; init; }
    public string SectionTitle { get; init; } = string.Empty;
    public string SectionTypeName { get; init; } = string.Empty;
    public List<SectionItemTranslationItemDto> Translations { get; init; } = [];
}

public record SearchSectionItemResponseDto
{
    public int Id { get; init; }
    public int SectionId { get; init; }
    public string Title { get; init; } = default!;
    public int Priority { get; init; }
    public string? Icon { get; init; }
    public string? ImageUrl { get; init; }
    public string? Url { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; }
}

public record GetAllSectionItemRequestDto : IContentPolicyQueryDto<SectionItem>
{
    [QueryFilter]
    public int? SectionId { get; init; }

    public string? Title { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<SectionItem, bool>>? ContentFilter { get; set; }
}

public record SearchSectionItemRequestDto : IContentPolicyQueryDto<SectionItem>
{
    [QueryFilter]
    public int? SectionId { get; init; }

    public string? Title { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<SectionItem, bool>>? ContentFilter { get; set; }
}

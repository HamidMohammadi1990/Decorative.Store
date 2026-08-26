using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Localization;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.SectionTypes;

public record GetAllSectionTypeResponseDto
{
    public int Id { get; init; }
    public bool IsActive { get; init; }
    public List<SectionTypeTranslationItemDto> Translations { get; init; } = [];
}

public record SearchSectionTypeResponseDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
}

public record GetAllSectionTypeRequestDto : IContentPolicyQueryDto<SectionType>
{
    public string? Name { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<SectionType, bool>>? ContentFilter { get; set; }
}

public record SearchSectionTypeRequestDto : IContentPolicyQueryDto<SectionType>
{
    public string? Name { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<SectionType, bool>>? ContentFilter { get; set; }
}

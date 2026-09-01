using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.MarketingPromos;

public record GetAllMarketingPromoRequestDto : IContentPolicyQueryDto<MarketingPromo>
{
    [QueryFilter]
    public int? LanguageId { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter]
    public MarketingPromoType? PromoType { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<MarketingPromo, bool>>? ContentFilter { get; set; }
}

public record GetAllMarketingPromoResponseDto
{
    public int Id { get; init; }
    public int LanguageId { get; init; }
    public MarketingPromoType PromoType { get; init; }
    public string Title { get; init; } = default!;
    public string? Subtitle { get; init; }
    public string LinkLabel { get; init; } = default!;
    public string LinkHref { get; init; } = default!;
    public string ImageFileName { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
}

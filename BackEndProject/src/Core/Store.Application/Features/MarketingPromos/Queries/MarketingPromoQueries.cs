using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Edition.Application.Features.MarketingPromos.Queries;

public record GetAllMarketingPromoRequest
    : ContentPolicyRequest<MarketingPromo>, IRequest<OperationResult<PagedResult<GetAllMarketingPromoResponse>>>
{
    public int? LanguageId { get; init; }
    public string? Title { get; init; }
    public MarketingPromoType? PromoType { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}

public record GetAllMarketingPromoResponse
{
    [JsonConverter(typeof(MarketingPromoEncryptor))]
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

public record GetMarketingPromoRequest : IRequest<OperationResult<GetMarketingPromoResponse?>>
{
    [JsonConverter(typeof(MarketingPromoEncryptor))]
    public int Id { get; init; }
}

public record GetMarketingPromoResponse
{
    [JsonConverter(typeof(MarketingPromoEncryptor))]
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

public record GetMarketingPromoStripRequest : IRequest<OperationResult<GetMarketingPromoStripResponse>>
{
    public int LanguageId { get; init; }
}

public record GetMarketingPromoStripResponse
{
    public List<MarketingPromoStripTileResponse> Tiles { get; init; } = [];
    public string? Disclaimer { get; init; }
    public MarketingPromoStripLinkResponse? DisclaimerLink { get; init; }
}

public record MarketingPromoStripTileResponse
{
    [JsonConverter(typeof(MarketingPromoEncryptor))]
    public int Id { get; init; }

    public MarketingPromoType PromoType { get; init; }
    public string Title { get; init; } = default!;
    public string? Subtitle { get; init; }
    public string ImageUrl { get; init; } = default!;
    public string LinkLabel { get; init; } = default!;
    public string LinkHref { get; init; } = default!;
}

public record MarketingPromoStripLinkResponse
{
    public string Label { get; init; } = default!;
    public string Href { get; init; } = default!;
}

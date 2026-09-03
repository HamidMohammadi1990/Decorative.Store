using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.MarketingPromos.Commands;

public record CreateMarketingPromoRequest : IRequest<OperationResult<CreateMarketingPromoResponse>>
{
    public int LanguageId { get; init; }
    public MarketingPromoType PromoType { get; init; }
    public string Title { get; init; } = default!;
    public string? Subtitle { get; init; }
    public string LinkLabel { get; init; } = default!;
    public string LinkHref { get; init; } = default!;
    public string ImageFileName { get; init; } = default!;
    public int Priority { get; init; }
}

public record CreateMarketingPromoResponse
{
    [JsonConverter(typeof(MarketingPromoEncryptor))]
    public int Id { get; init; }
}

public record UpdateMarketingPromoRequest : IRequest<OperationResult>
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

public record DeleteMarketingPromoRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(MarketingPromoEncryptor))]
    public int Id { get; init; }
}

public record UpdateMarketingStripDisclaimerRequest : IRequest<OperationResult>
{
    public int LanguageId { get; init; }
    public string? Disclaimer { get; init; }
    public string? DisclaimerLinkLabel { get; init; }
    public string? DisclaimerLinkHref { get; init; }
}

public record UploadMarketingPromoImageResponse
{
    public string ImageFileName { get; init; } = default!;
    public string ImageUrl { get; init; } = default!;
}

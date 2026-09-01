using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class MarketingPromo : BaseEntity
{
    public int LanguageId { get; private set; }
    public MarketingPromoType PromoType { get; private set; }
    public string Title { get; private set; } = default!;
    public string? Subtitle { get; private set; }
    public string LinkLabel { get; private set; } = default!;
    public string LinkHref { get; private set; } = default!;
    public string ImageFileName { get; private set; } = default!;
    public int Priority { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Language Language { get; private set; } = default!;

    public static MarketingPromo Create(
        int languageId,
        MarketingPromoType promoType,
        string title,
        string? subtitle,
        string linkLabel,
        string linkHref,
        string imageFileName,
        int priority)
        => new()
        {
            LanguageId = languageId,
            PromoType = promoType,
            Title = title,
            Subtitle = subtitle,
            LinkLabel = linkLabel,
            LinkHref = linkHref,
            ImageFileName = imageFileName,
            Priority = priority,
        };

    public void Update(
        int languageId,
        MarketingPromoType promoType,
        string title,
        string? subtitle,
        string linkLabel,
        string linkHref,
        string imageFileName,
        int priority,
        bool isActive)
    {
        LanguageId = languageId;
        PromoType = promoType;
        Title = title;
        Subtitle = subtitle;
        LinkLabel = linkLabel;
        LinkHref = linkHref;
        ImageFileName = imageFileName;
        Priority = priority;
        IsActive = isActive;
    }
}

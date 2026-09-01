using Store.Domain.Enums;

namespace Store.Infrastructure.Persistence.SeedData;

internal static class MarketingPromoSeedData
{
    internal sealed record PromoSeedItem(
        MarketingPromoType PromoType,
        string Title,
        string? Subtitle,
        string LinkLabel,
        string LinkHref,
        string ImageFileName,
        int Priority);

    internal sealed record DisclaimerSeedItem(
        string? Disclaimer,
        string? DisclaimerLinkLabel,
        string? DisclaimerLinkHref);

    public static IReadOnlyList<PromoSeedItem> FaPromos { get; } =
    [
        new(MarketingPromoType.Sale, "تا ۲۰٪ تخفیف اتاق خواب", "مبلمان محبوب", "خرید", "/bedroom", "/images/home/bedroom-set.jpg", 1),
        new(MarketingPromoType.Sale, "صندلی ناهارخوری زیر ۳.۵ میلیون", null, "خرید", "/dining/chairs", "/images/home/dining.jpg", 2),
        new(MarketingPromoType.NewArrival, "مبل موجود در انبار", "تحویل ۱ تا ۳ هفته*", "خرید", "/in-stock/sofas", "/images/home/velvet-sofa.jpg", 3),
        new(MarketingPromoType.Sale, "حراج تابستانه", "تا ۵۰٪ تخفیف", "حراج", "/sale", "/images/home/living-room.jpg", 4),
    ];

    public static IReadOnlyList<PromoSeedItem> EnPromos { get; } =
    [
        new(MarketingPromoType.Sale, "Save up to 20% on bedroom", "Shop furniture favourites", "Shop now", "/bedroom", "/images/home/bedroom-set.jpg", 1),
        new(MarketingPromoType.Sale, "Dining chairs under 3.5M", null, "Shop now", "/dining/chairs", "/images/home/dining.jpg", 2),
        new(MarketingPromoType.NewArrival, "In-stock sofas & chairs", "Delivered in 1–3 weeks*", "Shop now", "/in-stock/sofas", "/images/home/velvet-sofa.jpg", 3),
        new(MarketingPromoType.Sale, "Summer Sale", "Save up to 50% off", "Shop sale", "/sale", "/images/home/living-room.jpg", 4),
    ];

    public static DisclaimerSeedItem FaDisclaimer { get; } =
        new("*برخی استثناها اعمال می‌شود.", "جزئیات", "/promo-terms");

    public static DisclaimerSeedItem EnDisclaimer { get; } =
        new("*Some exclusions apply.", "See details", "/promo-terms");
}

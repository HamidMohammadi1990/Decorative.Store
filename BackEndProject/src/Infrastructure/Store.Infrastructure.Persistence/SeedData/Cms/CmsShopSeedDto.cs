using System.Text.Json.Serialization;

namespace Store.Infrastructure.Persistence.SeedData.Cms;

internal sealed class CmsShopSeedDto
{
    public CmsPromoAnnouncementDto? PromoAnnouncement { get; set; }
    public CmsUtilityBarDto? UtilityBar { get; set; }
    public CmsSiteHeaderDto? Header { get; set; }
    public CmsDesignServicesDto? DesignServices { get; set; }
    public CmsHeroDto? Hero { get; set; }
    public CmsPromoTilesDto? PromoTiles { get; set; }
    public CmsCategoryNavDto? CategoryNav { get; set; }
    public CmsFeaturedShopDto? FeaturedShop { get; set; }
    public CmsFooterDto? Footer { get; set; }
}

internal sealed class CmsLinkDto
{
    public string Label { get; set; } = string.Empty;
    public string Href { get; set; } = string.Empty;
}

internal sealed class CmsImageDto
{
    public string Src { get; set; } = string.Empty;
    public string Alt { get; set; } = string.Empty;
}

internal sealed class CmsPromoAnnouncementDto
{
    public string Id { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public CmsLinkDto Link { get; set; } = new();
}

internal sealed class CmsUtilityBarDto
{
    public string PhoneLabel { get; set; } = string.Empty;
    public string PhoneHref { get; set; } = string.Empty;
    public List<CmsLinkDto> Links { get; set; } = [];
}

internal sealed class CmsSiteHeaderDto
{
    public string BrandLabel { get; set; } = string.Empty;
    public string SearchPlaceholder { get; set; } = string.Empty;
    public string AccountLabel { get; set; } = string.Empty;
    public string CartLabel { get; set; } = string.Empty;
}

internal sealed class CmsDesignServicesDto
{
    public string Title { get; set; } = string.Empty;
    public CmsLinkDto Cta { get; set; } = new();
    public List<CmsLinkDto> FeaturedLinks { get; set; } = [];
}

internal sealed class CmsHeroDto
{
    public List<CmsHeroSlideDto> Slides { get; set; } = [];
}

internal sealed class CmsHeroSlideDto
{
    public string Id { get; set; } = string.Empty;
    public string? Eyebrow { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public CmsImageDto Image { get; set; } = new();
    public CmsLinkDto Cta { get; set; } = new();
}

internal sealed class CmsPromoTilesDto
{
    public List<CmsPromoTileDto> Tiles { get; set; } = [];
    public string? Disclaimer { get; set; }
    public CmsLinkDto? DisclaimerLink { get; set; }
}

internal sealed class CmsPromoTileDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public CmsImageDto Image { get; set; } = new();
    public CmsLinkDto Link { get; set; } = new();
}

internal sealed class CmsCategoryNavDto
{
    public List<CmsLinkDto> Items { get; set; } = [];
}

internal sealed class CmsFeaturedShopDto
{
    public List<CmsFeaturedShopSectionDto> Sections { get; set; } = [];
}

internal sealed class CmsFeaturedShopSectionDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public CmsImageDto Image { get; set; } = new();
    public CmsLinkDto Link { get; set; } = new();
}

internal sealed class CmsFooterDto
{
    public List<CmsFooterColumnDto> Columns { get; set; } = [];
    public string NewsletterTitle { get; set; } = string.Empty;
    public string NewsletterPlaceholder { get; set; } = string.Empty;
    public string NewsletterButton { get; set; } = string.Empty;
    public string Copyright { get; set; } = string.Empty;
    public List<CmsLinkDto> LegalLinks { get; set; } = [];
}

internal sealed class CmsFooterColumnDto
{
    public string Title { get; set; } = string.Empty;
    public List<CmsLinkDto> Links { get; set; } = [];
}

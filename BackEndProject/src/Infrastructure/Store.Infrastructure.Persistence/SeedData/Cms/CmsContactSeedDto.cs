namespace Store.Infrastructure.Persistence.SeedData.Cms;

internal sealed class CmsContactSeedDto
{
    public CmsPromoAnnouncementDto? PromoAnnouncement { get; set; }
    public CmsUtilityBarDto? UtilityBar { get; set; }
    public CmsSiteHeaderDto? Header { get; set; }
    public CmsFooterDto? Footer { get; set; }
    public CmsContactHeroDto? ContactHero { get; set; }
    public CmsContactMethodsDto? ContactMethods { get; set; }
    public CmsContactLocationsDto? ContactLocations { get; set; }
    public CmsContactFormIntroDto? ContactFormIntro { get; set; }
}

internal sealed class CmsContactHeroDto
{
    public string? Eyebrow { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
}

internal sealed class CmsContactMethodsDto
{
    public string Heading { get; set; } = string.Empty;
    public List<CmsContactMethodItemDto> Items { get; set; } = [];
}

internal sealed class CmsContactMethodItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Href { get; set; } = string.Empty;
}

internal sealed class CmsContactLocationsDto
{
    public string Heading { get; set; } = string.Empty;
    public List<CmsContactLocationItemDto> Items { get; set; } = [];
}

internal sealed class CmsContactLocationItemDto
{
    public string Title { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Hours { get; set; } = string.Empty;
    public string MapHref { get; set; } = string.Empty;
}

internal sealed class CmsContactFormIntroDto
{
    public string Heading { get; set; } = string.Empty;
    public string? Lead { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Note { get; set; }
}

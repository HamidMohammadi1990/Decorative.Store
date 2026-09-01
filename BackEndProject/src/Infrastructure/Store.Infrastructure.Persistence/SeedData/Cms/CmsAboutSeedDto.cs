namespace Store.Infrastructure.Persistence.SeedData.Cms;

internal sealed class CmsAboutSeedDto
{
    public CmsPromoAnnouncementDto? PromoAnnouncement { get; set; }
    public CmsUtilityBarDto? UtilityBar { get; set; }
    public CmsSiteHeaderDto? Header { get; set; }
    public CmsFooterDto? Footer { get; set; }
    public CmsAboutHeroDto? AboutHero { get; set; }
    public CmsAboutStoryDto? AboutStory { get; set; }
    public CmsAboutStatsDto? AboutStats { get; set; }
    public CmsAboutValuesDto? AboutValues { get; set; }
    public CmsAboutTimelineDto? AboutTimeline { get; set; }
    public CmsAboutCtaDto? AboutCta { get; set; }
}

internal sealed class CmsAboutHeroDto
{
    public string? Eyebrow { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public CmsImageDto Image { get; set; } = new();
    public CmsLinkDto Cta { get; set; } = new();
}

internal sealed class CmsAboutStoryDto
{
    public string Heading { get; set; } = string.Empty;
    public string Lead { get; set; } = string.Empty;
    public List<string> Paragraphs { get; set; } = [];
    public CmsImageDto Image { get; set; } = new();
}

internal sealed class CmsAboutStatsDto
{
    public string Heading { get; set; } = string.Empty;
    public List<CmsAboutStatItemDto> Items { get; set; } = [];
}

internal sealed class CmsAboutStatItemDto
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}

internal sealed class CmsAboutValuesDto
{
    public string Heading { get; set; } = string.Empty;
    public List<CmsAboutValueItemDto> Items { get; set; } = [];
}

internal sealed class CmsAboutValueItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

internal sealed class CmsAboutTimelineDto
{
    public string Heading { get; set; } = string.Empty;
    public List<CmsAboutTimelineItemDto> Items { get; set; } = [];
}

internal sealed class CmsAboutTimelineItemDto
{
    public string Year { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}

internal sealed class CmsAboutCtaDto
{
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public CmsLinkDto Cta { get; set; } = new();
}

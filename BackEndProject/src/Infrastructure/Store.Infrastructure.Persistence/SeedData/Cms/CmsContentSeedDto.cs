namespace Store.Infrastructure.Persistence.SeedData.Cms;

internal sealed class CmsContentSeedDto
{
    public CmsContentPageMetaDto? Page { get; set; }
    public CmsPromoAnnouncementDto? PromoAnnouncement { get; set; }
    public CmsUtilityBarDto? UtilityBar { get; set; }
    public CmsSiteHeaderDto? Header { get; set; }
    public CmsFooterDto? Footer { get; set; }
    public CmsContentHeroDto? ContentHero { get; set; }
    public List<CmsContentBodyBlockDto> ContentBlocks { get; set; } = [];
    public CmsContentStepsDto? ContentSteps { get; set; }
    public CmsContentCtaDto? ContentCta { get; set; }
}

internal sealed class CmsContentPageMetaDto
{
    public string Title { get; set; } = string.Empty;
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
}

internal sealed class CmsContentHeroDto
{
    public string? Eyebrow { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
}

internal sealed class CmsContentBodyBlockDto
{
    public string Heading { get; set; } = string.Empty;
    public string? Lead { get; set; }
    public List<string> Paragraphs { get; set; } = [];
}

internal sealed class CmsContentStepsDto
{
    public string Heading { get; set; } = string.Empty;
    public List<CmsContentStepItemDto> Items { get; set; } = [];
}

internal sealed class CmsContentStepItemDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

internal sealed class CmsContentCtaDto
{
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public CmsLinkDto PrimaryCta { get; set; } = new();
    public CmsLinkDto? SecondaryCta { get; set; }
}

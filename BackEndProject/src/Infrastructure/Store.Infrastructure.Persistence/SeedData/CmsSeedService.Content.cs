using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Constants;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Infrastructure.Persistence.SeedData.Cms;

namespace Store.Infrastructure.Persistence.SeedData;

public static partial class CmsSeedService
{
    private static readonly string[] ContentPageSlugs =
    [
        "privacy",
        "terms",
        "legal",
        "returns",
        "delivery",
        "promo-terms",
        "sustainability",
        "careers",
        "stores",
        "design-services",
    ];

    public static async Task SeedContentPagesAsync(EditionDbContext context, CancellationToken cancellationToken = default)
    {
        var faLanguage = await context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == "fa-IR", cancellationToken);

        var enLanguage = await context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == "en-US", cancellationToken);

        if (faLanguage is null || enLanguage is null)
            return;

        var typeIds = await EnsureSectionTypesAsync(context, enLanguage.Id, cancellationToken);

        foreach (var slug in ContentPageSlugs)
        {
            await SeedSingleContentPageAsync(
                context,
                slug,
                typeIds,
                faLanguage.Id,
                enLanguage.Id,
                cancellationToken);
        }
    }

    private static async Task SeedSingleContentPageAsync(
        EditionDbContext context,
        string slug,
        Dictionary<string, int> typeIds,
        int faLanguageId,
        int enLanguageId,
        CancellationToken cancellationToken)
    {
        var pageId = await context.PageTranslation
            .Where(x => x.Slug == slug && x.LanguageId == faLanguageId)
            .Select(x => x.PageId)
            .FirstOrDefaultAsync(cancellationToken);

        if (pageId != 0 && await HasCompleteContentHeroAsync(context, pageId, typeIds, faLanguageId, cancellationToken))
            return;

        var enContent = await LoadContentSeedAsync(slug, "en", cancellationToken);
        var faContent = await LoadContentSeedAsync(slug, "fa", cancellationToken);
        if (enContent is null && faContent is null)
            return;

        enContent ??= new CmsContentSeedDto();
        faContent ??= new CmsContentSeedDto();

        var faPage = faContent.Page ?? new CmsContentPageMetaDto { Title = slug };
        var enPage = enContent.Page ?? new CmsContentPageMetaDto { Title = slug };

        Page page;
        var priority = 0;

        if (pageId != 0)
        {
            await RemoveAllPageSectionsAsync(context, pageId, cancellationToken);

            page = await context.Page
                .Include(p => p.Translations)
                .FirstAsync(p => p.Id == pageId, cancellationToken);

            page.UpsertTranslation(
                faLanguageId,
                faPage.Title,
                slug,
                faPage.MetaTitle ?? faPage.Title,
                faPage.MetaDescription ?? faPage.Title);

            page.UpsertTranslation(
                enLanguageId,
                enPage.Title,
                slug,
                enPage.MetaTitle ?? enPage.Title,
                enPage.MetaDescription ?? enPage.Title);

            await context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            page = Page.Create(PageType.General, true);
            context.Page.Add(page);
            await context.SaveChangesAsync(cancellationToken);

            page.UpsertTranslation(
                faLanguageId,
                faPage.Title,
                slug,
                faPage.MetaTitle ?? faPage.Title,
                faPage.MetaDescription ?? faPage.Title);

            page.UpsertTranslation(
                enLanguageId,
                enPage.Title,
                slug,
                enPage.MetaTitle ?? enPage.Title,
                enPage.MetaDescription ?? enPage.Title);

            await context.SaveChangesAsync(cancellationToken);
        }

        var currentPageId = page.Id;

        await AddPromoAnnouncementAsync(
            context, currentPageId, priority++, typeIds, faLanguageId, enLanguageId,
            faContent.PromoAnnouncement, enContent.PromoAnnouncement, cancellationToken);

        await AddUtilityBarAsync(
            context, currentPageId, priority++, typeIds, faLanguageId, enLanguageId,
            faContent.UtilityBar, enContent.UtilityBar, cancellationToken);

        await AddSiteHeaderAsync(
            context, currentPageId, priority++, typeIds, faLanguageId, enLanguageId,
            faContent.Header, enContent.Header, cancellationToken);

        await AddContentHeroAsync(
            context, currentPageId, priority++, typeIds, faLanguageId, enLanguageId,
            faContent.ContentHero, enContent.ContentHero, cancellationToken);

        var blockCount = Math.Max(faContent.ContentBlocks.Count, enContent.ContentBlocks.Count);
        for (var index = 0; index < blockCount; index++)
        {
            var faBlock = index < faContent.ContentBlocks.Count ? faContent.ContentBlocks[index] : new CmsContentBodyBlockDto();
            var enBlock = index < enContent.ContentBlocks.Count ? enContent.ContentBlocks[index] : new CmsContentBodyBlockDto();

            await AddContentBodyBlockAsync(
                context, currentPageId, priority++, typeIds, faLanguageId, enLanguageId,
                faBlock, enBlock, cancellationToken);
        }

        if (faContent.ContentSteps is not null || enContent.ContentSteps is not null)
        {
            await AddContentStepsAsync(
                context, currentPageId, priority++, typeIds, faLanguageId, enLanguageId,
                faContent.ContentSteps, enContent.ContentSteps, cancellationToken);
        }

        if (faContent.ContentCta is not null || enContent.ContentCta is not null)
        {
            await AddContentCtaAsync(
                context, currentPageId, priority++, typeIds, faLanguageId, enLanguageId,
                faContent.ContentCta, enContent.ContentCta, cancellationToken);
        }

        if (faContent.Footer is not null || enContent.Footer is not null)
        {
            var faFooter = faContent.Footer ?? new CmsFooterDto();
            var enFooter = enContent.Footer ?? new CmsFooterDto();
            var columnCount = Math.Max(faFooter.Columns.Count, enFooter.Columns.Count);

            for (var index = 0; index < columnCount; index++)
            {
                var faColumn = index < faFooter.Columns.Count ? faFooter.Columns[index] : new CmsFooterColumnDto();
                var enColumn = index < enFooter.Columns.Count ? enFooter.Columns[index] : new CmsFooterColumnDto();

                await AddFooterColumnAsync(
                    context, currentPageId, priority++, typeIds, faLanguageId, enLanguageId,
                    faColumn, enColumn, cancellationToken);
            }

            await AddSiteFooterAsync(
                context, currentPageId, priority, typeIds, faLanguageId, enLanguageId,
                faFooter, enFooter, cancellationToken);
        }
    }

    private static async Task<bool> HasCompleteContentHeroAsync(
        EditionDbContext context,
        int pageId,
        IReadOnlyDictionary<string, int> typeIds,
        int faLanguageId,
        CancellationToken cancellationToken)
    {
        if (!typeIds.TryGetValue(CmsSectionTypeNames.ContentHero, out var heroTypeId))
            return false;

        var heroTitle = await context.PageSection
            .AsNoTracking()
            .Where(ps => ps.PageId == pageId && ps.Section.SectionTypeId == heroTypeId)
            .SelectMany(ps => ps.Section.Translations)
            .Where(t => t.LanguageId == faLanguageId)
            .Select(t => t.Title)
            .FirstOrDefaultAsync(cancellationToken);

        return !string.IsNullOrWhiteSpace(heroTitle);
    }

    private static async Task<CmsContentSeedDto?> LoadContentSeedAsync(
        string slug,
        string locale,
        CancellationToken cancellationToken)
    {
        var jsonPath = Path.Combine(AppContext.BaseDirectory, "SeedData", "cms", $"{slug}.{locale}.json");
        if (!File.Exists(jsonPath))
            return null;

        var json = await File.ReadAllTextAsync(jsonPath, cancellationToken);
        var page = JsonSerializer.Deserialize<CmsContentSeedDto>(json, JsonOptions);
        if (page is null)
            return null;

        if (page.Footer is not null)
            return page;

        var chromePath = Path.Combine(AppContext.BaseDirectory, "SeedData", "cms", $"chrome.{locale}.json");
        if (!File.Exists(chromePath))
            return page;

        var chromeJson = await File.ReadAllTextAsync(chromePath, cancellationToken);
        var chrome = JsonSerializer.Deserialize<CmsContentSeedDto>(chromeJson, JsonOptions);
        if (chrome is null)
            return page;

        page.PromoAnnouncement ??= chrome.PromoAnnouncement;
        page.UtilityBar ??= chrome.UtilityBar;
        page.Header ??= chrome.Header;
        page.Footer ??= chrome.Footer;

        return page;
    }

    private static async Task AddContentHeroAsync(
        EditionDbContext context,
        int pageId,
        int priority,
        Dictionary<string, int> typeIds,
        int faLanguageId,
        int enLanguageId,
        CmsContentHeroDto? faData,
        CmsContentHeroDto? enData,
        CancellationToken cancellationToken)
    {
        faData ??= new CmsContentHeroDto();
        enData ??= new CmsContentHeroDto();

        var section = Section.Create(typeIds[CmsSectionTypeNames.ContentHero], null, null, null, null, true);

        section.UpsertTranslation(
            faLanguageId,
            faData.Title,
            string.Empty,
            CmsSeedDescription.BuildAboutHeroDescription(faData.Eyebrow, faData.Subtitle, string.Empty));

        section.UpsertTranslation(
            enLanguageId,
            enData.Title,
            string.Empty,
            CmsSeedDescription.BuildAboutHeroDescription(enData.Eyebrow, enData.Subtitle, string.Empty));

        await AddPageSectionAsync(context, pageId, priority, section, [], cancellationToken);
    }

    private static async Task AddContentBodyBlockAsync(
        EditionDbContext context,
        int pageId,
        int priority,
        Dictionary<string, int> typeIds,
        int faLanguageId,
        int enLanguageId,
        CmsContentBodyBlockDto faData,
        CmsContentBodyBlockDto enData,
        CancellationToken cancellationToken)
    {
        var section = Section.Create(typeIds[CmsSectionTypeNames.ContentBodyBlock], null, null, null, null, true);
        section.UpsertTranslation(faLanguageId, faData.Heading, string.Empty, faData.Lead);
        section.UpsertTranslation(enLanguageId, enData.Heading, string.Empty, enData.Lead);

        var paragraphCount = Math.Max(faData.Paragraphs.Count, enData.Paragraphs.Count);
        var items = new List<SectionItem>();
        for (var index = 0; index < paragraphCount; index++)
        {
            var faParagraph = index < faData.Paragraphs.Count ? faData.Paragraphs[index] : string.Empty;
            var enParagraph = index < enData.Paragraphs.Count ? enData.Paragraphs[index] : string.Empty;
            items.Add(CreateTranslatedItemWithDescription(
                index,
                "paragraph",
                string.Empty,
                string.Empty,
                faParagraph,
                enParagraph,
                faLanguageId,
                enLanguageId));
        }

        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);
    }

    private static async Task AddContentStepsAsync(
        EditionDbContext context,
        int pageId,
        int priority,
        Dictionary<string, int> typeIds,
        int faLanguageId,
        int enLanguageId,
        CmsContentStepsDto? faData,
        CmsContentStepsDto? enData,
        CancellationToken cancellationToken)
    {
        faData ??= new CmsContentStepsDto();
        enData ??= new CmsContentStepsDto();

        var section = Section.Create(typeIds[CmsSectionTypeNames.ContentStepsGrid], null, null, null, null, true);
        section.UpsertTranslation(faLanguageId, faData.Heading, string.Empty);
        section.UpsertTranslation(enLanguageId, enData.Heading, string.Empty);

        var itemCount = Math.Max(faData.Items.Count, enData.Items.Count);
        var items = new List<SectionItem>();
        for (var index = 0; index < itemCount; index++)
        {
            var faItem = index < faData.Items.Count ? faData.Items[index] : new CmsContentStepItemDto();
            var enItem = index < enData.Items.Count ? enData.Items[index] : new CmsContentStepItemDto();
            items.Add(CreateTranslatedItemWithDescription(
                index,
                "step",
                faItem.Title,
                enItem.Title,
                faItem.Description,
                enItem.Description,
                faLanguageId,
                enLanguageId));
        }

        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);
    }

    private static async Task AddContentCtaAsync(
        EditionDbContext context,
        int pageId,
        int priority,
        Dictionary<string, int> typeIds,
        int faLanguageId,
        int enLanguageId,
        CmsContentCtaDto? faData,
        CmsContentCtaDto? enData,
        CancellationToken cancellationToken)
    {
        faData ??= new CmsContentCtaDto();
        enData ??= new CmsContentCtaDto();

        var section = Section.Create(typeIds[CmsSectionTypeNames.ContentCtaStrip], null, null, null, null, true);
        section.UpsertTranslation(faLanguageId, faData.Title, faData.PrimaryCta.Href, faData.Subtitle);
        section.UpsertTranslation(enLanguageId, enData.Title, enData.PrimaryCta.Href, enData.Subtitle);

        var items = new List<SectionItem>
        {
            CreateTranslatedItem(0, "cta", faData.PrimaryCta.Label, enData.PrimaryCta.Label, faLanguageId, enLanguageId, faData.PrimaryCta.Href, enData.PrimaryCta.Href),
        };

        if (!string.IsNullOrWhiteSpace(faData.SecondaryCta?.Label) || !string.IsNullOrWhiteSpace(enData.SecondaryCta?.Label))
        {
            items.Add(CreateTranslatedItem(
                1,
                "cta-secondary",
                faData.SecondaryCta?.Label ?? string.Empty,
                enData.SecondaryCta?.Label ?? string.Empty,
                faLanguageId,
                enLanguageId,
                faData.SecondaryCta?.Href ?? string.Empty,
                enData.SecondaryCta?.Href ?? string.Empty));
        }

        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);
    }
}

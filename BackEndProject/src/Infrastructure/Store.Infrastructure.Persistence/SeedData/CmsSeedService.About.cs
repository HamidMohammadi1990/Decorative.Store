using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Constants;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Infrastructure.Persistence.SeedData.Cms;

namespace Store.Infrastructure.Persistence.SeedData;

public static partial class CmsSeedService
{
    public static async Task SeedAboutPagesAsync(EditionDbContext context, CancellationToken cancellationToken = default)
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

        var aboutPageId = await context.PageTranslation
            .Where(x => x.Slug == "about" && x.LanguageId == faLanguage.Id)
            .Select(x => x.PageId)
            .FirstOrDefaultAsync(cancellationToken);

        if (aboutPageId != 0 && await HasCompleteAboutContentAsync(context, aboutPageId, typeIds, faLanguage.Id, cancellationToken))
            return;

        var enContent = await LoadAboutContentAsync("en", cancellationToken);
        var faContent = await LoadAboutContentAsync("fa", cancellationToken);
        if (enContent is null && faContent is null)
            return;

        enContent ??= new CmsAboutSeedDto();
        faContent ??= new CmsAboutSeedDto();

        Page page;
        var priority = 0;

        if (aboutPageId != 0)
        {
            await RemoveAllPageSectionsAsync(context, aboutPageId, cancellationToken);

            page = await context.Page
                .Include(p => p.Translations)
                .FirstAsync(p => p.Id == aboutPageId, cancellationToken);

            page.UpsertTranslation(
                faLanguage.Id,
                "درباره ما",
                "about",
                "درباره دیبا گالری",
                "داستان، ارزش‌ها و تیم دیبا گالری — فروشگاه مبلمان و دکوراسیون.");

            page.UpsertTranslation(
                enLanguage.Id,
                "About Us",
                "about",
                "About Diba Gallery",
                "Our story, values, and team — a design-led furniture and home décor destination.");

            await context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            page = Page.Create(PageType.General, true);
            context.Page.Add(page);
            await context.SaveChangesAsync(cancellationToken);

            page.UpsertTranslation(
                faLanguage.Id,
                "درباره ما",
                "about",
                "درباره دیبا گالری",
                "داستان، ارزش‌ها و تیم دیبا گالری — فروشگاه مبلمان و دکوراسیون.");

            page.UpsertTranslation(
                enLanguage.Id,
                "About Us",
                "about",
                "About Diba Gallery",
                "Our story, values, and team — a design-led furniture and home décor destination.");

            await context.SaveChangesAsync(cancellationToken);
        }

        var pageId = page.Id;

        await AddPromoAnnouncementAsync(
            context, pageId, priority++, typeIds, faLanguage.Id, enLanguage.Id,
            faContent.PromoAnnouncement, enContent.PromoAnnouncement, cancellationToken);

        await AddUtilityBarAsync(
            context, pageId, priority++, typeIds, faLanguage.Id, enLanguage.Id,
            faContent.UtilityBar, enContent.UtilityBar, cancellationToken);

        await AddSiteHeaderAsync(
            context, pageId, priority++, typeIds, faLanguage.Id, enLanguage.Id,
            faContent.Header, enContent.Header, cancellationToken);

        await AddAboutHeroAsync(
            context, pageId, priority++, typeIds, faLanguage.Id, enLanguage.Id,
            faContent.AboutHero, enContent.AboutHero, cancellationToken);

        await AddAboutStoryAsync(
            context, pageId, priority++, typeIds, faLanguage.Id, enLanguage.Id,
            faContent.AboutStory, enContent.AboutStory, cancellationToken);

        await AddAboutStatsAsync(
            context, pageId, priority++, typeIds, faLanguage.Id, enLanguage.Id,
            faContent.AboutStats, enContent.AboutStats, cancellationToken);

        await AddAboutValuesAsync(
            context, pageId, priority++, typeIds, faLanguage.Id, enLanguage.Id,
            faContent.AboutValues, enContent.AboutValues, cancellationToken);

        await AddAboutTimelineAsync(
            context, pageId, priority++, typeIds, faLanguage.Id, enLanguage.Id,
            faContent.AboutTimeline, enContent.AboutTimeline, cancellationToken);

        await AddAboutCtaAsync(
            context, pageId, priority++, typeIds, faLanguage.Id, enLanguage.Id,
            faContent.AboutCta, enContent.AboutCta, cancellationToken);

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
                    context, pageId, priority++, typeIds, faLanguage.Id, enLanguage.Id,
                    faColumn, enColumn, cancellationToken);
            }

            await AddSiteFooterAsync(
                context, pageId, priority, typeIds, faLanguage.Id, enLanguage.Id,
                faFooter, enFooter, cancellationToken);
        }
    }

    private static async Task<bool> HasCompleteAboutContentAsync(
        EditionDbContext context,
        int pageId,
        IReadOnlyDictionary<string, int> typeIds,
        int faLanguageId,
        CancellationToken cancellationToken)
    {
        if (!typeIds.TryGetValue(CmsSectionTypeNames.AboutHero, out var aboutHeroTypeId))
            return false;

        var heroTitle = await context.PageSection
            .AsNoTracking()
            .Where(ps => ps.PageId == pageId && ps.Section.SectionTypeId == aboutHeroTypeId)
            .SelectMany(ps => ps.Section.Translations)
            .Where(t => t.LanguageId == faLanguageId)
            .Select(t => t.Title)
            .FirstOrDefaultAsync(cancellationToken);

        return !string.IsNullOrWhiteSpace(heroTitle);
    }

    private static async Task RemoveAllPageSectionsAsync(
        EditionDbContext context,
        int pageId,
        CancellationToken cancellationToken)
    {
        var pageSections = await context.PageSection
            .Include(ps => ps.Section)
                .ThenInclude(s => s.SectionItems)
            .Include(ps => ps.Section)
                .ThenInclude(s => s.Translations)
            .Where(ps => ps.PageId == pageId)
            .ToListAsync(cancellationToken);

        foreach (var pageSection in pageSections)
        {
            context.PageSection.Remove(pageSection);
            context.SectionItem.RemoveRange(pageSection.Section.SectionItems);
            context.Section.Remove(pageSection.Section);
        }

        if (pageSections.Count > 0)
            await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task<CmsAboutSeedDto?> LoadAboutContentAsync(string locale, CancellationToken cancellationToken)
    {
        var jsonPath = Path.Combine(AppContext.BaseDirectory, "SeedData", "cms", $"about.{locale}.json");
        if (!File.Exists(jsonPath))
            return null;

        var json = await File.ReadAllTextAsync(jsonPath, cancellationToken);
        return JsonSerializer.Deserialize<CmsAboutSeedDto>(json, JsonOptions);
    }

    private static async Task AddAboutHeroAsync(
        EditionDbContext context,
        int pageId,
        int priority,
        Dictionary<string, int> typeIds,
        int faLanguageId,
        int enLanguageId,
        CmsAboutHeroDto? faData,
        CmsAboutHeroDto? enData,
        CancellationToken cancellationToken)
    {
        faData ??= new CmsAboutHeroDto();
        enData ??= new CmsAboutHeroDto();

        var section = Section.Create(
            typeIds[CmsSectionTypeNames.AboutHero],
            null,
            faData.Image.Src,
            null,
            null,
            true);

        section.UpsertTranslation(
            faLanguageId,
            faData.Title,
            faData.Cta.Href,
            CmsSeedDescription.BuildAboutHeroDescription(faData.Eyebrow, faData.Subtitle, faData.Cta.Label));

        section.UpsertTranslation(
            enLanguageId,
            enData.Title,
            enData.Cta.Href,
            CmsSeedDescription.BuildAboutHeroDescription(enData.Eyebrow, enData.Subtitle, enData.Cta.Label));

        var items = new List<SectionItem>
        {
            CreateTranslatedItem(0, "cta", faData.Cta.Label, enData.Cta.Label, faLanguageId, enLanguageId, faData.Cta.Href, enData.Cta.Href),
        };

        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);
    }

    private static async Task AddAboutStoryAsync(
        EditionDbContext context,
        int pageId,
        int priority,
        Dictionary<string, int> typeIds,
        int faLanguageId,
        int enLanguageId,
        CmsAboutStoryDto? faData,
        CmsAboutStoryDto? enData,
        CancellationToken cancellationToken)
    {
        faData ??= new CmsAboutStoryDto();
        enData ??= new CmsAboutStoryDto();

        var section = Section.Create(
            typeIds[CmsSectionTypeNames.AboutStoryBlock],
            null,
            faData.Image.Src,
            null,
            null,
            true);

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

    private static async Task AddAboutStatsAsync(
        EditionDbContext context,
        int pageId,
        int priority,
        Dictionary<string, int> typeIds,
        int faLanguageId,
        int enLanguageId,
        CmsAboutStatsDto? faData,
        CmsAboutStatsDto? enData,
        CancellationToken cancellationToken)
    {
        faData ??= new CmsAboutStatsDto();
        enData ??= new CmsAboutStatsDto();

        var section = Section.Create(typeIds[CmsSectionTypeNames.AboutStatsStrip], null, null, null, null, true);
        section.UpsertTranslation(faLanguageId, faData.Heading, string.Empty);
        section.UpsertTranslation(enLanguageId, enData.Heading, string.Empty);

        var itemCount = Math.Max(faData.Items.Count, enData.Items.Count);
        var items = new List<SectionItem>();
        for (var index = 0; index < itemCount; index++)
        {
            var faItem = index < faData.Items.Count ? faData.Items[index] : new CmsAboutStatItemDto();
            var enItem = index < enData.Items.Count ? enData.Items[index] : new CmsAboutStatItemDto();
            items.Add(CreateTranslatedItemWithDescription(
                index,
                "stat",
                faItem.Value,
                enItem.Value,
                faItem.Label,
                enItem.Label,
                faLanguageId,
                enLanguageId));
        }

        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);
    }

    private static async Task AddAboutValuesAsync(
        EditionDbContext context,
        int pageId,
        int priority,
        Dictionary<string, int> typeIds,
        int faLanguageId,
        int enLanguageId,
        CmsAboutValuesDto? faData,
        CmsAboutValuesDto? enData,
        CancellationToken cancellationToken)
    {
        faData ??= new CmsAboutValuesDto();
        enData ??= new CmsAboutValuesDto();

        var section = Section.Create(typeIds[CmsSectionTypeNames.AboutValuesGrid], null, null, null, null, true);
        section.UpsertTranslation(faLanguageId, faData.Heading, string.Empty);
        section.UpsertTranslation(enLanguageId, enData.Heading, string.Empty);

        var itemCount = Math.Max(faData.Items.Count, enData.Items.Count);
        var items = new List<SectionItem>();
        for (var index = 0; index < itemCount; index++)
        {
            var faItem = index < faData.Items.Count ? faData.Items[index] : new CmsAboutValueItemDto();
            var enItem = index < enData.Items.Count ? enData.Items[index] : new CmsAboutValueItemDto();
            items.Add(CreateTranslatedItemWithDescription(
                index,
                faItem.Id,
                faItem.Title,
                enItem.Title,
                faItem.Description,
                enItem.Description,
                faLanguageId,
                enLanguageId));
        }

        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);
    }

    private static async Task AddAboutTimelineAsync(
        EditionDbContext context,
        int pageId,
        int priority,
        Dictionary<string, int> typeIds,
        int faLanguageId,
        int enLanguageId,
        CmsAboutTimelineDto? faData,
        CmsAboutTimelineDto? enData,
        CancellationToken cancellationToken)
    {
        faData ??= new CmsAboutTimelineDto();
        enData ??= new CmsAboutTimelineDto();

        var section = Section.Create(typeIds[CmsSectionTypeNames.AboutTimeline], null, null, null, null, true);
        section.UpsertTranslation(faLanguageId, faData.Heading, string.Empty);
        section.UpsertTranslation(enLanguageId, enData.Heading, string.Empty);

        var itemCount = Math.Max(faData.Items.Count, enData.Items.Count);
        var items = new List<SectionItem>();
        for (var index = 0; index < itemCount; index++)
        {
            var faItem = index < faData.Items.Count ? faData.Items[index] : new CmsAboutTimelineItemDto();
            var enItem = index < enData.Items.Count ? enData.Items[index] : new CmsAboutTimelineItemDto();
            items.Add(CreateTranslatedItemWithDescription(
                index,
                "milestone",
                faItem.Year,
                enItem.Year,
                faItem.Text,
                enItem.Text,
                faLanguageId,
                enLanguageId));
        }

        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);
    }

    private static async Task AddAboutCtaAsync(
        EditionDbContext context,
        int pageId,
        int priority,
        Dictionary<string, int> typeIds,
        int faLanguageId,
        int enLanguageId,
        CmsAboutCtaDto? faData,
        CmsAboutCtaDto? enData,
        CancellationToken cancellationToken)
    {
        faData ??= new CmsAboutCtaDto();
        enData ??= new CmsAboutCtaDto();

        var section = Section.Create(typeIds[CmsSectionTypeNames.AboutCtaStrip], null, null, null, null, true);
        section.UpsertTranslation(faLanguageId, faData.Title, faData.Cta.Href, faData.Subtitle);
        section.UpsertTranslation(enLanguageId, enData.Title, enData.Cta.Href, enData.Subtitle);

        var items = new List<SectionItem>
        {
            CreateTranslatedItem(0, "cta", faData.Cta.Label, enData.Cta.Label, faLanguageId, enLanguageId, faData.Cta.Href, enData.Cta.Href),
        };

        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);
    }

    private static SectionItem CreateTranslatedItemWithDescription(
        int priority,
        string? icon,
        string faTitle,
        string enTitle,
        string? faDescription,
        string? enDescription,
        int faLanguageId,
        int enLanguageId,
        string? faUrl = null,
        string? enUrl = null)
    {
        var item = SectionItem.Create(0, priority, icon, null, true);
        item.UpsertTranslation(faLanguageId, faTitle, faDescription, faUrl);
        item.UpsertTranslation(enLanguageId, enTitle, enDescription, enUrl);
        return item;
    }
}

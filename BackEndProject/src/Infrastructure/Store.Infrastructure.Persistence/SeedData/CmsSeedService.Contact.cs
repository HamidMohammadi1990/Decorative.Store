using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Constants;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Infrastructure.Persistence.SeedData.Cms;

namespace Store.Infrastructure.Persistence.SeedData;

public static partial class CmsSeedService
{
    public static async Task SeedContactPagesAsync(EditionDbContext context, CancellationToken cancellationToken = default)
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

        var contactPageId = await context.PageTranslation
            .Where(x => x.Slug == "contact" && x.LanguageId == faLanguage.Id)
            .Select(x => x.PageId)
            .FirstOrDefaultAsync(cancellationToken);

        if (contactPageId != 0 && await HasCompleteContactContentAsync(context, contactPageId, typeIds, faLanguage.Id, cancellationToken))
            return;

        var enContent = await LoadContactContentAsync("en", cancellationToken);
        var faContent = await LoadContactContentAsync("fa", cancellationToken);
        if (enContent is null && faContent is null)
            return;

        enContent ??= new CmsContactSeedDto();
        faContent ??= new CmsContactSeedDto();

        Page page;
        var priority = 0;

        if (contactPageId != 0)
        {
            await RemoveAllPageSectionsAsync(context, contactPageId, cancellationToken);

            page = await context.Page
                .Include(p => p.Translations)
                .FirstAsync(p => p.Id == contactPageId, cancellationToken);

            page.UpsertTranslation(
                faLanguage.Id,
                "تماس با ما",
                "contact",
                "تماس با دیبا گالری",
                "پشتیبانی، شعب و راه‌های ارتباط با تیم دیبا گالری.");

            page.UpsertTranslation(
                enLanguage.Id,
                "Contact Us",
                "contact",
                "Contact Diba Gallery",
                "Support, store locations, and ways to reach our team.");

            await context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            page = Page.Create(PageType.General, true);
            context.Page.Add(page);
            await context.SaveChangesAsync(cancellationToken);

            page.UpsertTranslation(
                faLanguage.Id,
                "تماس با ما",
                "contact",
                "تماس با دیبا گالری",
                "پشتیبانی، شعب و راه‌های ارتباط با تیم دیبا گالری.");

            page.UpsertTranslation(
                enLanguage.Id,
                "Contact Us",
                "contact",
                "Contact Diba Gallery",
                "Support, store locations, and ways to reach our team.");

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

        await AddContactHeroAsync(
            context, pageId, priority++, typeIds, faLanguage.Id, enLanguage.Id,
            faContent.ContactHero, enContent.ContactHero, cancellationToken);

        await AddContactMethodsAsync(
            context, pageId, priority++, typeIds, faLanguage.Id, enLanguage.Id,
            faContent.ContactMethods, enContent.ContactMethods, cancellationToken);

        await AddContactLocationsAsync(
            context, pageId, priority++, typeIds, faLanguage.Id, enLanguage.Id,
            faContent.ContactLocations, enContent.ContactLocations, cancellationToken);

        await AddContactFormIntroAsync(
            context, pageId, priority++, typeIds, faLanguage.Id, enLanguage.Id,
            faContent.ContactFormIntro, enContent.ContactFormIntro, cancellationToken);

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

    private static async Task<bool> HasCompleteContactContentAsync(
        EditionDbContext context,
        int pageId,
        IReadOnlyDictionary<string, int> typeIds,
        int faLanguageId,
        CancellationToken cancellationToken)
    {
        if (!typeIds.TryGetValue(CmsSectionTypeNames.ContactHero, out var contactHeroTypeId))
            return false;

        var heroTitle = await context.PageSection
            .AsNoTracking()
            .Where(ps => ps.PageId == pageId && ps.Section.SectionTypeId == contactHeroTypeId)
            .SelectMany(ps => ps.Section.Translations)
            .Where(t => t.LanguageId == faLanguageId)
            .Select(t => t.Title)
            .FirstOrDefaultAsync(cancellationToken);

        return !string.IsNullOrWhiteSpace(heroTitle);
    }

    private static async Task<CmsContactSeedDto?> LoadContactContentAsync(string locale, CancellationToken cancellationToken)
    {
        var jsonPath = Path.Combine(AppContext.BaseDirectory, "SeedData", "cms", $"contact.{locale}.json");
        if (!File.Exists(jsonPath))
            return null;

        var json = await File.ReadAllTextAsync(jsonPath, cancellationToken);
        return JsonSerializer.Deserialize<CmsContactSeedDto>(json, JsonOptions);
    }

    private static async Task AddContactHeroAsync(
        EditionDbContext context,
        int pageId,
        int priority,
        Dictionary<string, int> typeIds,
        int faLanguageId,
        int enLanguageId,
        CmsContactHeroDto? faData,
        CmsContactHeroDto? enData,
        CancellationToken cancellationToken)
    {
        faData ??= new CmsContactHeroDto();
        enData ??= new CmsContactHeroDto();

        var section = Section.Create(
            typeIds[CmsSectionTypeNames.ContactHero],
            null,
            null,
            null,
            null,
            true);

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

    private static async Task AddContactMethodsAsync(
        EditionDbContext context,
        int pageId,
        int priority,
        Dictionary<string, int> typeIds,
        int faLanguageId,
        int enLanguageId,
        CmsContactMethodsDto? faData,
        CmsContactMethodsDto? enData,
        CancellationToken cancellationToken)
    {
        faData ??= new CmsContactMethodsDto();
        enData ??= new CmsContactMethodsDto();

        var section = Section.Create(typeIds[CmsSectionTypeNames.ContactMethodsGrid], null, null, null, null, true);
        section.UpsertTranslation(faLanguageId, faData.Heading, string.Empty);
        section.UpsertTranslation(enLanguageId, enData.Heading, string.Empty);

        var itemCount = Math.Max(faData.Items.Count, enData.Items.Count);
        var items = new List<SectionItem>();
        for (var index = 0; index < itemCount; index++)
        {
            var faItem = index < faData.Items.Count ? faData.Items[index] : new CmsContactMethodItemDto();
            var enItem = index < enData.Items.Count ? enData.Items[index] : new CmsContactMethodItemDto();
            items.Add(CreateTranslatedItemWithDescription(
                index,
                faItem.Id,
                faItem.Title,
                enItem.Title,
                faItem.Description,
                enItem.Description,
                faLanguageId,
                enLanguageId,
                faItem.Href,
                enItem.Href));
        }

        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);
    }

    private static async Task AddContactLocationsAsync(
        EditionDbContext context,
        int pageId,
        int priority,
        Dictionary<string, int> typeIds,
        int faLanguageId,
        int enLanguageId,
        CmsContactLocationsDto? faData,
        CmsContactLocationsDto? enData,
        CancellationToken cancellationToken)
    {
        faData ??= new CmsContactLocationsDto();
        enData ??= new CmsContactLocationsDto();

        var section = Section.Create(typeIds[CmsSectionTypeNames.ContactLocationsGrid], null, null, null, null, true);
        section.UpsertTranslation(faLanguageId, faData.Heading, string.Empty);
        section.UpsertTranslation(enLanguageId, enData.Heading, string.Empty);

        var itemCount = Math.Max(faData.Items.Count, enData.Items.Count);
        var items = new List<SectionItem>();
        for (var index = 0; index < itemCount; index++)
        {
            var faItem = index < faData.Items.Count ? faData.Items[index] : new CmsContactLocationItemDto();
            var enItem = index < enData.Items.Count ? enData.Items[index] : new CmsContactLocationItemDto();
            var faDescription = string.Join('|', new[] { faItem.Address, faItem.Hours }.Where(x => !string.IsNullOrWhiteSpace(x)));
            var enDescription = string.Join('|', new[] { enItem.Address, enItem.Hours }.Where(x => !string.IsNullOrWhiteSpace(x)));
            items.Add(CreateTranslatedItemWithDescription(
                index,
                "location",
                faItem.Title,
                enItem.Title,
                faDescription,
                enDescription,
                faLanguageId,
                enLanguageId,
                faItem.MapHref,
                enItem.MapHref));
        }

        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);
    }

    private static async Task AddContactFormIntroAsync(
        EditionDbContext context,
        int pageId,
        int priority,
        Dictionary<string, int> typeIds,
        int faLanguageId,
        int enLanguageId,
        CmsContactFormIntroDto? faData,
        CmsContactFormIntroDto? enData,
        CancellationToken cancellationToken)
    {
        faData ??= new CmsContactFormIntroDto();
        enData ??= new CmsContactFormIntroDto();

        var section = Section.Create(typeIds[CmsSectionTypeNames.ContactFormIntro], null, null, null, null, true);
        section.UpsertTranslation(faLanguageId, faData.Heading, faData.Email, faData.Lead);
        section.UpsertTranslation(enLanguageId, enData.Heading, enData.Email, enData.Lead);

        var items = new List<SectionItem>();
        if (!string.IsNullOrWhiteSpace(faData.Note) || !string.IsNullOrWhiteSpace(enData.Note))
        {
            items.Add(CreateTranslatedItemWithDescription(
                0,
                "note",
                string.Empty,
                string.Empty,
                faData.Note ?? string.Empty,
                enData.Note ?? string.Empty,
                faLanguageId,
                enLanguageId));
        }

        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);
    }
}

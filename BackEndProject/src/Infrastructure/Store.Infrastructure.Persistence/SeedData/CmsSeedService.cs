using System.Text.Json;

using Microsoft.EntityFrameworkCore;

using Store.Domain.Constants;

using Store.Domain.Entities;

using Store.Domain.Enums;

using Store.Infrastructure.Persistence.SeedData.Cms;



namespace Store.Infrastructure.Persistence.SeedData;



public static class CmsSeedService

{

    private static readonly JsonSerializerOptions JsonOptions = new()

    {

        PropertyNameCaseInsensitive = true,

    };



    private static readonly string[] SectionTypeNames =

    [

        CmsSectionTypeNames.PromoAnnouncement,

        CmsSectionTypeNames.UtilityBar,

        CmsSectionTypeNames.SiteHeader,

        CmsSectionTypeNames.DesignServicesStrip,

        CmsSectionTypeNames.HeroCarousel,

        CmsSectionTypeNames.PromoTileStrip,

        CmsSectionTypeNames.CategoryNav,

        CmsSectionTypeNames.FeaturedShopGrid,

        CmsSectionTypeNames.FooterColumn,

        CmsSectionTypeNames.SiteFooter,

    ];



    public static async Task SeedShopPagesAsync(EditionDbContext context, CancellationToken cancellationToken = default)

    {

        var faLanguage = await context.Language

            .AsNoTracking()

            .FirstOrDefaultAsync(x => x.Code == "fa-IR", cancellationToken);



        var enLanguage = await context.Language

            .AsNoTracking()

            .FirstOrDefaultAsync(x => x.Code == "en-US", cancellationToken);



        if (faLanguage is null || enLanguage is null)

            return;



        if (await context.PageTranslation.AnyAsync(

                x => x.Slug == "shop" && x.LanguageId == faLanguage.Id,

                cancellationToken))

            return;



        var typeIds = await EnsureSectionTypesAsync(context, enLanguage.Id, cancellationToken);



        var enContent = await LoadShopContentAsync("en", cancellationToken);

        var faContent = await LoadShopContentAsync("fa", cancellationToken);

        if (enContent is null && faContent is null)

            return;



        enContent ??= new CmsShopSeedDto();

        faContent ??= new CmsShopSeedDto();



        var page = Page.Create(PageType.Home, true);

        context.Page.Add(page);

        await context.SaveChangesAsync(cancellationToken);



        page.UpsertTranslation(

            faLanguage.Id,

            "فروشگاه",

            "shop",

            "فروشگاه",

            "فروشگاه");

        page.UpsertTranslation(

            enLanguage.Id,

            "Shop",

            "shop",

            "Shop",

            "Shop");

        await context.SaveChangesAsync(cancellationToken);



        var priority = 0;

        await AddPromoAnnouncementAsync(

            context, page.Id, priority++, typeIds, faLanguage.Id, enLanguage.Id,

            faContent.PromoAnnouncement, enContent.PromoAnnouncement, cancellationToken);



        await AddUtilityBarAsync(

            context, page.Id, priority++, typeIds, faLanguage.Id, enLanguage.Id,

            faContent.UtilityBar, enContent.UtilityBar, cancellationToken);



        await AddSiteHeaderAsync(

            context, page.Id, priority++, typeIds, faLanguage.Id, enLanguage.Id,

            faContent.Header, enContent.Header, cancellationToken);



        await AddDesignServicesAsync(

            context, page.Id, priority++, typeIds, faLanguage.Id, enLanguage.Id,

            faContent.DesignServices, enContent.DesignServices, cancellationToken);



        await AddHeroAsync(

            context, page.Id, priority++, typeIds, faLanguage.Id, enLanguage.Id,

            faContent.Hero, enContent.Hero, cancellationToken);



        await AddPromoTilesAsync(

            context, page.Id, priority++, typeIds, faLanguage.Id, enLanguage.Id,

            faContent.PromoTiles, enContent.PromoTiles, cancellationToken);



        await AddCategoryNavAsync(

            context, page.Id, priority++, typeIds, faLanguage.Id, enLanguage.Id,

            faContent.CategoryNav, enContent.CategoryNav, cancellationToken);



        await AddFeaturedShopAsync(

            context, page.Id, priority++, typeIds, faLanguage.Id, enLanguage.Id,

            faContent.FeaturedShop, enContent.FeaturedShop, cancellationToken);



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

                    context, page.Id, priority++, typeIds, faLanguage.Id, enLanguage.Id,

                    faColumn, enColumn, cancellationToken);

            }



            await AddSiteFooterAsync(

                context, page.Id, priority, typeIds, faLanguage.Id, enLanguage.Id,

                faFooter, enFooter, cancellationToken);

        }

    }



    public static async Task SeedBlogPagesAsync(EditionDbContext context, CancellationToken cancellationToken = default)

    {

        var faLanguage = await context.Language

            .AsNoTracking()

            .FirstOrDefaultAsync(x => x.Code == "fa-IR", cancellationToken);



        var enLanguage = await context.Language

            .AsNoTracking()

            .FirstOrDefaultAsync(x => x.Code == "en-US", cancellationToken);



        if (faLanguage is null || enLanguage is null)

            return;



        if (await context.PageTranslation.AnyAsync(

                x => x.Slug == "blog" && x.LanguageId == faLanguage.Id,

                cancellationToken))

            return;



        var typeIds = await EnsureSectionTypesAsync(context, enLanguage.Id, cancellationToken);



        var enContent = await LoadBlogContentAsync("en", cancellationToken);

        var faContent = await LoadBlogContentAsync("fa", cancellationToken);

        if (enContent is null && faContent is null)

            return;



        enContent ??= new CmsShopSeedDto();

        faContent ??= new CmsShopSeedDto();



        var page = Page.Create(PageType.General, true);

        context.Page.Add(page);

        await context.SaveChangesAsync(cancellationToken);



        page.UpsertTranslation(

            faLanguage.Id,

            "مجله",

            "blog",

            "مجله",

            "مجله");

        page.UpsertTranslation(

            enLanguage.Id,

            "Journal",

            "blog",

            "Journal",

            "Journal");

        await context.SaveChangesAsync(cancellationToken);



        var priority = 0;



        await AddPromoAnnouncementAsync(

            context, page.Id, priority++, typeIds, faLanguage.Id, enLanguage.Id,

            faContent.PromoAnnouncement, enContent.PromoAnnouncement, cancellationToken);



        await AddUtilityBarAsync(

            context, page.Id, priority++, typeIds, faLanguage.Id, enLanguage.Id,

            faContent.UtilityBar, enContent.UtilityBar, cancellationToken);



        await AddSiteHeaderAsync(

            context, page.Id, priority++, typeIds, faLanguage.Id, enLanguage.Id,

            faContent.Header, enContent.Header, cancellationToken);



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

                    context, page.Id, priority++, typeIds, faLanguage.Id, enLanguage.Id,

                    faColumn, enColumn, cancellationToken);

            }



            await AddSiteFooterAsync(

                context, page.Id, priority, typeIds, faLanguage.Id, enLanguage.Id,

                faFooter, enFooter, cancellationToken);

        }

    }



    private static async Task<CmsShopSeedDto?> LoadShopContentAsync(string locale, CancellationToken cancellationToken)

    {

        var jsonPath = Path.Combine(AppContext.BaseDirectory, "SeedData", "cms", $"shop.{locale}.json");

        if (!File.Exists(jsonPath))

            return null;



        var json = await File.ReadAllTextAsync(jsonPath, cancellationToken);

        return JsonSerializer.Deserialize<CmsShopSeedDto>(json, JsonOptions);

    }



    private static async Task<CmsShopSeedDto?> LoadBlogContentAsync(string locale, CancellationToken cancellationToken)

    {

        var jsonPath = Path.Combine(AppContext.BaseDirectory, "SeedData", "cms", $"blog.{locale}.json");

        if (!File.Exists(jsonPath))

            return null;



        var json = await File.ReadAllTextAsync(jsonPath, cancellationToken);

        return JsonSerializer.Deserialize<CmsShopSeedDto>(json, JsonOptions);

    }



    private static async Task<Dictionary<string, int>> EnsureSectionTypesAsync(

        EditionDbContext context,

        int englishLanguageId,

        CancellationToken cancellationToken)

    {

        var map = await context.SectionTypeTranslation

            .AsNoTracking()

            .Where(x => x.LanguageId == englishLanguageId && SectionTypeNames.Contains(x.Name))

            .ToDictionaryAsync(x => x.Name, x => x.SectionTypeId, cancellationToken);



        foreach (var name in SectionTypeNames)

        {

            if (map.ContainsKey(name))

                continue;



            var sectionType = SectionType.Create(true);

            sectionType.UpsertTranslation(englishLanguageId, name);

            context.SectionType.Add(sectionType);

            await context.SaveChangesAsync(cancellationToken);

            map[name] = sectionType.Id;

        }



        return map;

    }



    private static async Task AddPageSectionAsync(

        EditionDbContext context,

        int pageId,

        int priority,

        Section section,

        List<SectionItem> items,

        CancellationToken cancellationToken)

    {

        context.Section.Add(section);

        await context.SaveChangesAsync(cancellationToken);



        foreach (var item in items)

        {

            var persistedItem = SectionItem.Create(

                section.Id,

                item.Priority,

                item.Icon,

                item.ImageUrl,

                item.IsActive);



            foreach (var translation in item.Translations)

            {

                persistedItem.UpsertTranslation(

                    translation.LanguageId,

                    translation.Title,

                    translation.Description,

                    translation.Url);

            }



            context.SectionItem.Add(persistedItem);

        }



        context.PageSection.Add(PageSection.Create(pageId, section.Id, priority));

        await context.SaveChangesAsync(cancellationToken);

    }



    private static async Task AddPromoAnnouncementAsync(

        EditionDbContext context,

        int pageId,

        int priority,

        Dictionary<string, int> typeIds,

        int faLanguageId,

        int enLanguageId,

        CmsPromoAnnouncementDto? faData,

        CmsPromoAnnouncementDto? enData,

        CancellationToken cancellationToken)

    {

        faData ??= new CmsPromoAnnouncementDto();

        enData ??= new CmsPromoAnnouncementDto();



        var section = Section.Create(typeIds[CmsSectionTypeNames.PromoAnnouncement], null, null, null, null, true);

        section.UpsertTranslation(faLanguageId, faData.Id, faData.Link.Href, faData.Message);

        section.UpsertTranslation(enLanguageId, enData.Id, enData.Link.Href, enData.Message);



        var items = new List<SectionItem>();

        if (!string.IsNullOrWhiteSpace(faData.Link.Label) || !string.IsNullOrWhiteSpace(enData.Link.Label))

        {

            var item = SectionItem.Create(0, 0, faData.Id ?? enData.Id, null, true);

            if (!string.IsNullOrWhiteSpace(faData.Link.Label))

                item.UpsertTranslation(faLanguageId, faData.Link.Label);

            if (!string.IsNullOrWhiteSpace(enData.Link.Label))

                item.UpsertTranslation(enLanguageId, enData.Link.Label);

            items.Add(item);

        }



        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);

    }



    private static async Task AddUtilityBarAsync(

        EditionDbContext context,

        int pageId,

        int priority,

        Dictionary<string, int> typeIds,

        int faLanguageId,

        int enLanguageId,

        CmsUtilityBarDto? faData,

        CmsUtilityBarDto? enData,

        CancellationToken cancellationToken)

    {

        faData ??= new CmsUtilityBarDto();

        enData ??= new CmsUtilityBarDto();



        var section = Section.Create(typeIds[CmsSectionTypeNames.UtilityBar], null, null, null, null, true);

        section.UpsertTranslation(faLanguageId, faData.PhoneLabel, faData.PhoneHref);

        section.UpsertTranslation(enLanguageId, enData.PhoneLabel, enData.PhoneHref);



        var items = BuildPairedLinkItems(faData.Links, enData.Links, faLanguageId, enLanguageId);

        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);

    }



    private static async Task AddSiteHeaderAsync(

        EditionDbContext context,

        int pageId,

        int priority,

        Dictionary<string, int> typeIds,

        int faLanguageId,

        int enLanguageId,

        CmsSiteHeaderDto? faData,

        CmsSiteHeaderDto? enData,

        CancellationToken cancellationToken)

    {

        faData ??= new CmsSiteHeaderDto();

        enData ??= new CmsSiteHeaderDto();



        var section = Section.Create(typeIds[CmsSectionTypeNames.SiteHeader], null, null, null, null, true);

        section.UpsertTranslation(faLanguageId, faData.BrandLabel, string.Empty);

        section.UpsertTranslation(enLanguageId, enData.BrandLabel, string.Empty);



        var items = new List<SectionItem>

        {

            CreateTranslatedItem(0, "search", faData.SearchPlaceholder, enData.SearchPlaceholder, faLanguageId, enLanguageId),

            CreateTranslatedItem(1, "account", faData.AccountLabel, enData.AccountLabel, faLanguageId, enLanguageId),

            CreateTranslatedItem(2, "cart", faData.CartLabel, enData.CartLabel, faLanguageId, enLanguageId),

        };



        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);

    }



    private static async Task AddDesignServicesAsync(

        EditionDbContext context,

        int pageId,

        int priority,

        Dictionary<string, int> typeIds,

        int faLanguageId,

        int enLanguageId,

        CmsDesignServicesDto? faData,

        CmsDesignServicesDto? enData,

        CancellationToken cancellationToken)

    {

        faData ??= new CmsDesignServicesDto();

        enData ??= new CmsDesignServicesDto();



        var section = Section.Create(typeIds[CmsSectionTypeNames.DesignServicesStrip], null, null, null, null, true);

        section.UpsertTranslation(faLanguageId, faData.Title, string.Empty);

        section.UpsertTranslation(enLanguageId, enData.Title, string.Empty);



        var items = new List<SectionItem>

        {

            CreateTranslatedItem(0, "cta", faData.Cta.Label, enData.Cta.Label, faLanguageId, enLanguageId, faData.Cta.Href, enData.Cta.Href),

        };

        items.AddRange(BuildPairedLinkItems(faData.FeaturedLinks, enData.FeaturedLinks, faLanguageId, enLanguageId, startPriority: 1));



        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);

    }



    private static async Task AddHeroAsync(

        EditionDbContext context,

        int pageId,

        int priority,

        Dictionary<string, int> typeIds,

        int faLanguageId,

        int enLanguageId,

        CmsHeroDto? faData,

        CmsHeroDto? enData,

        CancellationToken cancellationToken)

    {

        faData ??= new CmsHeroDto();

        enData ??= new CmsHeroDto();



        var section = Section.Create(typeIds[CmsSectionTypeNames.HeroCarousel], null, null, null, null, true);

        section.UpsertTranslation(faLanguageId, "Hero", string.Empty);

        section.UpsertTranslation(enLanguageId, "Hero", string.Empty);



        var slideCount = Math.Max(faData.Slides.Count, enData.Slides.Count);

        var items = new List<SectionItem>();



        for (var index = 0; index < slideCount; index++)

        {

            var faSlide = index < faData.Slides.Count ? faData.Slides[index] : new CmsHeroSlideDto();

            var enSlide = index < enData.Slides.Count ? enData.Slides[index] : new CmsHeroSlideDto();

            var faDescription = BuildSlideDescription(faSlide);

            var enDescription = BuildSlideDescription(enSlide);



            var item = SectionItem.Create(0, index, faSlide.Id ?? enSlide.Id, faSlide.Image.Src ?? enSlide.Image.Src, true);

            item.UpsertTranslation(faLanguageId, faSlide.Title, faDescription, faSlide.Cta.Href);

            item.UpsertTranslation(enLanguageId, enSlide.Title, enDescription, enSlide.Cta.Href);

            items.Add(item);

        }



        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);

    }



    private static async Task AddPromoTilesAsync(

        EditionDbContext context,

        int pageId,

        int priority,

        Dictionary<string, int> typeIds,

        int faLanguageId,

        int enLanguageId,

        CmsPromoTilesDto? faData,

        CmsPromoTilesDto? enData,

        CancellationToken cancellationToken)

    {

        faData ??= new CmsPromoTilesDto();

        enData ??= new CmsPromoTilesDto();



        var section = Section.Create(typeIds[CmsSectionTypeNames.PromoTileStrip], null, null, null, null, true);

        section.UpsertTranslation(

            faLanguageId,

            "Promo tiles",

            faData.DisclaimerLink?.Href ?? string.Empty,

            faData.Disclaimer);

        section.UpsertTranslation(

            enLanguageId,

            "Promo tiles",

            enData.DisclaimerLink?.Href ?? string.Empty,

            enData.Disclaimer);



        var tileCount = Math.Max(faData.Tiles.Count, enData.Tiles.Count);

        var items = new List<SectionItem>();



        for (var index = 0; index < tileCount; index++)

        {

            var faTile = index < faData.Tiles.Count ? faData.Tiles[index] : new CmsPromoTileDto();

            var enTile = index < enData.Tiles.Count ? enData.Tiles[index] : new CmsPromoTileDto();



            var item = SectionItem.Create(0, index, faTile.Id ?? enTile.Id, faTile.Image.Src ?? enTile.Image.Src, true);

            item.UpsertTranslation(

                faLanguageId,

                faTile.Title,

                CmsSeedDescription.BuildTileDescription(faTile.Subtitle, faTile.Link.Label),

                faTile.Link.Href);

            item.UpsertTranslation(

                enLanguageId,

                enTile.Title,

                CmsSeedDescription.BuildTileDescription(enTile.Subtitle, enTile.Link.Label),

                enTile.Link.Href);

            items.Add(item);

        }



        if (faData.DisclaimerLink is not null || enData.DisclaimerLink is not null)

        {

            var item = SectionItem.Create(0, 100, "disclaimer-link", null, true);

            if (!string.IsNullOrWhiteSpace(faData.DisclaimerLink?.Label))

                item.UpsertTranslation(faLanguageId, faData.DisclaimerLink.Label, url: faData.DisclaimerLink.Href);

            if (!string.IsNullOrWhiteSpace(enData.DisclaimerLink?.Label))

                item.UpsertTranslation(enLanguageId, enData.DisclaimerLink.Label, url: enData.DisclaimerLink.Href);

            items.Add(item);

        }



        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);

    }



    private static async Task AddCategoryNavAsync(

        EditionDbContext context,

        int pageId,

        int priority,

        Dictionary<string, int> typeIds,

        int faLanguageId,

        int enLanguageId,

        CmsCategoryNavDto? faData,

        CmsCategoryNavDto? enData,

        CancellationToken cancellationToken)

    {

        faData ??= new CmsCategoryNavDto();

        enData ??= new CmsCategoryNavDto();



        var section = Section.Create(typeIds[CmsSectionTypeNames.CategoryNav], null, null, null, null, true);

        section.UpsertTranslation(faLanguageId, "Category nav", string.Empty);

        section.UpsertTranslation(enLanguageId, "Category nav", string.Empty);



        var items = BuildPairedLinkItems(faData.Items, enData.Items, faLanguageId, enLanguageId);

        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);

    }



    private static async Task AddFeaturedShopAsync(

        EditionDbContext context,

        int pageId,

        int priority,

        Dictionary<string, int> typeIds,

        int faLanguageId,

        int enLanguageId,

        CmsFeaturedShopDto? faData,

        CmsFeaturedShopDto? enData,

        CancellationToken cancellationToken)

    {

        faData ??= new CmsFeaturedShopDto();

        enData ??= new CmsFeaturedShopDto();



        var section = Section.Create(typeIds[CmsSectionTypeNames.FeaturedShopGrid], null, null, null, null, true);

        section.UpsertTranslation(faLanguageId, "Featured shop", string.Empty);

        section.UpsertTranslation(enLanguageId, "Featured shop", string.Empty);



        var cardCount = Math.Max(faData.Sections.Count, enData.Sections.Count);

        var items = new List<SectionItem>();



        for (var index = 0; index < cardCount; index++)

        {

            var faCard = index < faData.Sections.Count ? faData.Sections[index] : new CmsFeaturedShopSectionDto();

            var enCard = index < enData.Sections.Count ? enData.Sections[index] : new CmsFeaturedShopSectionDto();



            var item = SectionItem.Create(0, index, faCard.Id ?? enCard.Id, faCard.Image.Src ?? enCard.Image.Src, true);

            item.UpsertTranslation(

                faLanguageId,

                faCard.Title,

                CmsSeedDescription.BuildFeaturedDescription(faCard.Subtitle, faCard.Link.Label),

                faCard.Link.Href);

            item.UpsertTranslation(

                enLanguageId,

                enCard.Title,

                CmsSeedDescription.BuildFeaturedDescription(enCard.Subtitle, enCard.Link.Label),

                enCard.Link.Href);

            items.Add(item);

        }



        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);

    }



    private static async Task AddFooterColumnAsync(

        EditionDbContext context,

        int pageId,

        int priority,

        Dictionary<string, int> typeIds,

        int faLanguageId,

        int enLanguageId,

        CmsFooterColumnDto faColumn,

        CmsFooterColumnDto enColumn,

        CancellationToken cancellationToken)

    {

        var section = Section.Create(typeIds[CmsSectionTypeNames.FooterColumn], null, null, null, null, true);

        section.UpsertTranslation(faLanguageId, faColumn.Title, string.Empty);

        section.UpsertTranslation(enLanguageId, enColumn.Title, string.Empty);



        var items = BuildPairedLinkItems(faColumn.Links, enColumn.Links, faLanguageId, enLanguageId);

        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);

    }



    private static async Task AddSiteFooterAsync(

        EditionDbContext context,

        int pageId,

        int priority,

        Dictionary<string, int> typeIds,

        int faLanguageId,

        int enLanguageId,

        CmsFooterDto faFooter,

        CmsFooterDto enFooter,

        CancellationToken cancellationToken)

    {

        var section = Section.Create(typeIds[CmsSectionTypeNames.SiteFooter], null, null, null, null, true);

        section.UpsertTranslation(

            faLanguageId,

            faFooter.NewsletterTitle,

            faFooter.NewsletterButton,

            faFooter.NewsletterPlaceholder);

        section.UpsertTranslation(

            enLanguageId,

            enFooter.NewsletterTitle,

            enFooter.NewsletterButton,

            enFooter.NewsletterPlaceholder);



        var items = new List<SectionItem>

        {

            CreateTranslatedItem(0, "copyright", faFooter.Copyright, enFooter.Copyright, faLanguageId, enLanguageId),

        };

        items.AddRange(BuildPairedLinkItems(faFooter.LegalLinks, enFooter.LegalLinks, faLanguageId, enLanguageId, startPriority: 1, icon: "legal"));



        await AddPageSectionAsync(context, pageId, priority, section, items, cancellationToken);

    }



    private static List<SectionItem> BuildPairedLinkItems(

        IReadOnlyList<CmsLinkDto> faLinks,

        IReadOnlyList<CmsLinkDto> enLinks,

        int faLanguageId,

        int enLanguageId,

        int startPriority = 0,

        string? icon = null)

    {

        var count = Math.Max(faLinks.Count, enLinks.Count);

        var items = new List<SectionItem>();



        for (var index = 0; index < count; index++)

        {

            var faLink = index < faLinks.Count ? faLinks[index] : new CmsLinkDto();

            var enLink = index < enLinks.Count ? enLinks[index] : new CmsLinkDto();

            var item = SectionItem.Create(0, startPriority + index, icon, null, true);

            item.UpsertTranslation(faLanguageId, faLink.Label, url: faLink.Href);

            item.UpsertTranslation(enLanguageId, enLink.Label, url: enLink.Href);

            items.Add(item);

        }



        return items;

    }



    private static SectionItem CreateTranslatedItem(

        int priority,

        string? icon,

        string faTitle,

        string enTitle,

        int faLanguageId,

        int enLanguageId,

        string? faUrl = null,

        string? enUrl = null)

    {

        var item = SectionItem.Create(0, priority, icon, null, true);

        item.UpsertTranslation(faLanguageId, faTitle, url: faUrl);

        item.UpsertTranslation(enLanguageId, enTitle, url: enUrl);

        return item;

    }



    private static string BuildSlideDescription(CmsHeroSlideDto slide)

    {

        var description = CmsSeedDescription.BuildSlideDescription(

            slide.Eyebrow,

            slide.Subtitle,

            slide.Cta.Label);



        if (!string.IsNullOrWhiteSpace(slide.Image.Alt))

        {

            description = string.IsNullOrWhiteSpace(description)

                ? $"alt:{slide.Image.Alt.Trim()}"

                : $"{description}|alt:{slide.Image.Alt.Trim()}";

        }



        return description;

    }

}



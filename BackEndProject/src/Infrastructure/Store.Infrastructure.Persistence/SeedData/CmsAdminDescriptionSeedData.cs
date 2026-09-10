using Microsoft.EntityFrameworkCore;
using Store.Domain.Constants;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Infrastructure.Persistence.SeedData;

internal static class CmsAdminDescriptionSeedData
{
    private static readonly Dictionary<PageType, string> PageTypeDescriptions = new()
    {
        [PageType.General] = "صفحه عمومی CMS — برای محتوای سفارشی با slug دلخواه.",
        [PageType.Home] = "صفحه خانه/فروشگاه — معمولاً slug=shop؛ شامل هیرو، پرومو، فوتر و …",
        [PageType.CategoryPage] = "صفحه دسته — قالب لیست محصولات یک دسته.",
        [PageType.ProductPage] = "صفحه محصول — قالب جزئیات محصول.",
        [PageType.CheckoutPage] = "صفحه تسویه — مراحل checkout.",
    };

    private static readonly Dictionary<string, string> SectionTypeDescriptions = new(StringComparer.Ordinal)
    {
        [CmsSectionTypeNames.PromoAnnouncement] = "اعلان/بنر تبلیغاتی بالای صفحه — پیام‌های کوتاه تخفیف یا اطلاع‌رسانی.",
        [CmsSectionTypeNames.UtilityBar] = "نوار ابزار بالای هدر — لینک‌های کمکی، زبان، حساب کاربری.",
        [CmsSectionTypeNames.SiteHeader] = "هدر اصلی سایت — لوگو، منوی ناوبری، جستجو.",
        [CmsSectionTypeNames.DesignServicesStrip] = "نوار خدمات طراحی — معرفی خدمات دکوراسیون و طراحی داخلی.",
        [CmsSectionTypeNames.HeroCarousel] = "اسلایدر هیرو — تصاویر/بنرهای بزرگ چرخشی بالای صفحه.",
        [CmsSectionTypeNames.PromoTileStrip] = "ردیف کاشی تبلیغاتی — چند بلوک تصویری/لینکی کنار هم.",
        [CmsSectionTypeNames.CategoryNav] = "ناوبری دسته‌بندی — لینک دسته‌های محصول.",
        [CmsSectionTypeNames.FeaturedShopGrid] = "گرید محصولات ویژه — نمایش محصولات یا دسته‌های برجسته.",
        [CmsSectionTypeNames.FooterColumn] = "ستون فوتر — یک ستون لینک یا متن در پاورقی.",
        [CmsSectionTypeNames.SiteFooter] = "فوتر کامل سایت — چند ستون، کپی‌رایت، لینک‌ها.",
        [CmsSectionTypeNames.AboutHero] = "هیرو صفحه درباره ما — بنر/عنوان اصلی.",
        [CmsSectionTypeNames.AboutStoryBlock] = "بلوک داستان — متن معرفی برند و تاریخچه.",
        [CmsSectionTypeNames.AboutStatsStrip] = "نوار آمار — اعداد و عنوان (مثلاً سال‌ها، مشتریان).",
        [CmsSectionTypeNames.AboutValuesGrid] = "گرید ارزش‌ها — اصول و ارزش‌های برند.",
        [CmsSectionTypeNames.AboutTimeline] = "خط زمانی — مراحل تاریخچه شرکت.",
        [CmsSectionTypeNames.AboutCtaStrip] = "بنر دعوت به اقدام در صفحه درباره ما.",
        [CmsSectionTypeNames.ContactHero] = "هیرو صفحه تماس — عنوان و مقدمه.",
        [CmsSectionTypeNames.ContactMethodsGrid] = "گرید روش‌های تماس — تلفن، ایمیل، شبکه‌های اجتماعی.",
        [CmsSectionTypeNames.ContactLocationsGrid] = "گرید آدرس/شعب — مکان‌های فیزیکی.",
        [CmsSectionTypeNames.ContactFormIntro] = "متن مقدمه فرم تماس — راهنمای ارسال پیام.",
        [CmsSectionTypeNames.ContentHero] = "هیرو صفحات محتوایی — Terms، Privacy و صفحات مشابه.",
        [CmsSectionTypeNames.ContentBodyBlock] = "بلوک متن بدنه — پاراگراف‌های محتوای اصلی.",
        [CmsSectionTypeNames.ContentStepsGrid] = "گرید مراحل — گام‌های فرآیند یا راهنما.",
        [CmsSectionTypeNames.ContentCtaStrip] = "بنر CTA در صفحات محتوایی — دعوت به اقدام.",
    };

    internal static async Task SeedAsync(EditionDbContext context, CancellationToken cancellationToken = default)
    {
        await SeedPageTypeGuidesAsync(context, cancellationToken);
        await SeedSectionTypeDescriptionsAsync(context, cancellationToken);
    }

    private static async Task SeedPageTypeGuidesAsync(EditionDbContext context, CancellationToken cancellationToken)
    {
        var existingTypes = await context.PageTypeGuide
            .AsNoTracking()
            .Select(x => x.Type)
            .ToListAsync(cancellationToken);

        foreach (var (pageType, description) in PageTypeDescriptions)
        {
            if (existingTypes.Contains(pageType))
                continue;

            context.PageTypeGuide.Add(PageTypeGuide.Create(pageType, description));
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedSectionTypeDescriptionsAsync(EditionDbContext context, CancellationToken cancellationToken)
    {
        var enLanguage = await context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == "en-US", cancellationToken);

        if (enLanguage is null)
            return;

        var sectionTypeNames = SectionTypeDescriptions.Keys.ToArray();

        var sectionTypes = await context.SectionType
            .Include(x => x.Translations)
            .Where(x => x.Translations.Any(t =>
                t.LanguageId == enLanguage.Id && sectionTypeNames.Contains(t.Name)))
            .ToListAsync(cancellationToken);

        var changed = false;

        foreach (var sectionType in sectionTypes)
        {
            var englishName = sectionType.Translations
                .FirstOrDefault(t => t.LanguageId == enLanguage.Id)
                ?.Name;

            if (englishName is null || !SectionTypeDescriptions.TryGetValue(englishName, out var description))
                continue;

            if (!string.IsNullOrWhiteSpace(sectionType.AdminDescription))
                continue;

            sectionType.Update(sectionType.IsActive, enLanguage.Id, englishName, description);
            changed = true;
        }

        if (changed)
            await context.SaveChangesAsync(cancellationToken);
    }
}

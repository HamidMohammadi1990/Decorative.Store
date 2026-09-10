using Edition.Application.Common.Directories;
using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Localization;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Pages.Queries;

public class GetPageBySlugHandler
    (IPageRepository pageRepository,
     ICurrentLanguageContext languageContext,
     ILanguageRegistry languageRegistry)
    : IRequestHandler<GetPageBySlugRequest, OperationResult<GetPageBySlugResponse>>
{
    public async Task<OperationResult<GetPageBySlugResponse>> Handle(
        GetPageBySlugRequest request,
        CancellationToken cancellationToken)
    {
        var page = await pageRepository.GetActiveBySlugWithContentAsync(request.Slug, cancellationToken);
        if (page is null)
            return new GetPageBySlugResponse { NotFound = true };

        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var defaultLanguageId = defaultLanguage.Id;
        var englishLanguage = await languageRegistry.GetByCodeAsync("en-US", cancellationToken);
        var sectionTypeKeyLanguageId = englishLanguage?.Id ?? defaultLanguageId;
        var utcNow = DateTime.UtcNow;

        var (pageTitle, pageSlug, metaTitle, metaDescription) = TranslationResolver.Resolve(
            page.Translations,
            languageId,
            defaultLanguageId);

        var sections = page.PageSections
            .Where(ps => ps.Section.IsVisibleAt(utcNow, ps.Section.SectionType.IsActive))
            .OrderBy(ps => ps.Priority)
            .Select(ps =>
            {
                var (sectionTitle, sectionDescription, sectionUrl) = TranslationResolver.Resolve(
                    ps.Section.Translations,
                    languageId,
                    defaultLanguageId);
                // Render contract keys (PromoAnnouncement, HeroCarousel, …) are stored on the English translation.
                var sectionTypeName = TranslationResolver.ResolveName(
                    ps.Section.SectionType.Translations,
                    sectionTypeKeyLanguageId,
                    sectionTypeKeyLanguageId);

                return new PageSectionRenderResponse
                {
                    SectionTypeId = ps.Section.SectionTypeId,
                    SectionTypeName = sectionTypeName,
                    Priority = ps.Priority,
                    Title = sectionTitle,
                    Description = sectionDescription,
                    Url = sectionUrl,
                    ImageUrl = CmsDirectory.ResolvePublicImageUrl(ps.Section.ImageUrl),
                    Items = ps.Section.SectionItems
                        .Where(i => i.IsActive)
                        .OrderBy(i => i.Priority)
                        .Select(i =>
                        {
                            var (itemTitle, itemDescription, itemUrl) = TranslationResolver.Resolve(
                                i.Translations,
                                languageId,
                                defaultLanguageId);

                            return new SectionItemRenderResponse
                            {
                                Title = itemTitle,
                                Priority = i.Priority,
                                Icon = i.Icon,
                                ImageUrl = CmsDirectory.ResolvePublicImageUrl(i.ImageUrl),
                                Url = itemUrl,
                                Description = itemDescription,
                            };
                        })
                        .ToList(),
                };
            })
            .ToList();

        return new GetPageBySlugResponse
        {
            NotFound = false,
            Slug = pageSlug,
            Title = pageTitle,
            Type = page.Type,
            MetaTitle = metaTitle,
            MetaDescription = metaDescription,
            Sections = sections,
        };
    }
}

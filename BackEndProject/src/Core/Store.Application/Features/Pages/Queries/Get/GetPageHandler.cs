using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Pages.Queries;

public class GetPageHandler
    (IPageRepository pageRepository,
     IPageMapperService mapper,
     ICurrentLanguageContext languageContext,
     ILanguageRegistry languageRegistry)
    : IRequestHandler<GetPageRequest, OperationResult<GetPageResponse?>>
{
    public async Task<OperationResult<GetPageResponse?>> Handle(GetPageRequest request, CancellationToken cancellationToken)
    {
        var page = await pageRepository.GetWithTranslationsAsNoTrackingAsync(request.Id, cancellationToken);
        if (page is null)
            return ErrorModel.Create("InvalidId");

        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var (title, slug, metaTitle, metaDescription) = TranslationResolver.Resolve(
            page.Translations,
            languageId,
            defaultLanguage.Id);

        return mapper.Map(page, title, slug, metaTitle, metaDescription);
    }
}

using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Categories.Queries;

public class GetCategoryHandler
    (ICategoryRepository categoryRepository,
     ICategoryMapperService mapper,
     ICurrentLanguageContext languageContext,
     ILanguageRegistry languageRegistry)
    : IRequestHandler<GetCategoryRequest, OperationResult<GetCategoryResponse?>>
{
    public async Task<OperationResult<GetCategoryResponse?>> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetWithTranslationsAsNoTrackingAsync(request.Id, cancellationToken);
        if (category is null)
            return ErrorModel.Create("InvalidId");

        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var (title, slug) = TranslationResolver.Resolve(category.Translations, languageId, defaultLanguage.Id);

        var result = mapper.Map(category, title, slug);
        return result;
    }
}

using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SubCategories.Queries;

public class GetSubCategoryHandler
    (ISubCategoryRepository subCategoryRepository,
     ISubCategoryMapperService mapper,
     ICurrentLanguageContext languageContext,
     ILanguageRegistry languageRegistry)
    : IRequestHandler<GetSubCategoryRequest, OperationResult<GetSubCategoryResponse?>>
{
    public async Task<OperationResult<GetSubCategoryResponse?>> Handle(GetSubCategoryRequest request, CancellationToken cancellationToken)
    {
        var subCategory = await subCategoryRepository.GetWithTranslationsAsNoTrackingAsync(request.Id, cancellationToken);
        if (subCategory is null)
            return ErrorModel.Create("InvalidId");

        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var (title, slug) = TranslationResolver.Resolve(subCategory.Translations, languageId, defaultLanguage.Id);

        var result = mapper.Map(subCategory, title, slug);
        return result;
    }
}

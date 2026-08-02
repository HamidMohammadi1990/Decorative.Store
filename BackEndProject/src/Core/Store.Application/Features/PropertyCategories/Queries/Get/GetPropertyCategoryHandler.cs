using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyCategories.Queries;

public class GetPropertyCategoryHandler
    (IPropertyCategoryRepository propertyCategoryRepository,
     IPropertyCategoryMapperService mapper,
     ICurrentLanguageContext languageContext,
     ILanguageRegistry languageRegistry)
    : IRequestHandler<GetPropertyCategoryRequest, OperationResult<GetPropertyCategoryResponse?>>
{
    public async Task<OperationResult<GetPropertyCategoryResponse?>> Handle(GetPropertyCategoryRequest request, CancellationToken cancellationToken)
    {
        var propertyCategory = await propertyCategoryRepository.GetWithTranslationsAsNoTrackingAsync(request.Id, cancellationToken);
        if (propertyCategory is null)
            return ErrorModel.Create("InvalidId");

        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var title = TranslationResolver.ResolveTitle(propertyCategory.Translations, languageId, defaultLanguage.Id);

        var result = mapper.Map(propertyCategory, title);
        return result;
    }
}

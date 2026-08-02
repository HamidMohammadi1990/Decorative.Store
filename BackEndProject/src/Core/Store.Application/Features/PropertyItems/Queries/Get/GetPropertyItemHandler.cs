using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyItems.Queries;

public class GetPropertyItemHandler
    (IPropertyItemRepository propertyItemRepository,
     IPropertyItemMapperService mapper,
     ICurrentLanguageContext languageContext,
     ILanguageRegistry languageRegistry)
    : IRequestHandler<GetPropertyItemRequest, OperationResult<GetPropertyItemResponse?>>
{
    public async Task<OperationResult<GetPropertyItemResponse?>> Handle(GetPropertyItemRequest request, CancellationToken cancellationToken)
    {
        var propertyItem = await propertyItemRepository.GetWithTranslationsAsNoTrackingAsync(request.Id, cancellationToken);
        if (propertyItem is null)
            return ErrorModel.Create("InvalidId");

        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var title = TranslationResolver.ResolveTitle(propertyItem.Translations, languageId, defaultLanguage.Id);

        var result = mapper.Map(propertyItem, title);
        return result;
    }
}

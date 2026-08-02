using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Properties.Queries;

public class GetPropertyHandler
    (IPropertyRepository propertyRepository,
     IPropertyMapperService mapper,
     ICurrentLanguageContext languageContext,
     ILanguageRegistry languageRegistry)
    : IRequestHandler<GetPropertyRequest, OperationResult<GetPropertyResponse?>>
{
    public async Task<OperationResult<GetPropertyResponse?>> Handle(GetPropertyRequest request, CancellationToken cancellationToken)
    {
        var property = await propertyRepository.GetWithTranslationsAsNoTrackingAsync(request.Id, cancellationToken);
        if (property is null)
            return ErrorModel.Create("InvalidId");

        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var (title, description) = TranslationResolver.Resolve(property.Translations, languageId, defaultLanguage.Id);

        var result = mapper.Map(property, title, description);
        return result;
    }
}

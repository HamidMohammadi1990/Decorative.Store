using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionTypes.Queries;

public class GetSectionTypeHandler
    (ISectionTypeRepository repository,
     ISectionTypeMapperService mapper,
     ICurrentLanguageContext languageContext,
     ILanguageRegistry languageRegistry)
    : IRequestHandler<GetSectionTypeRequest, OperationResult<GetSectionTypeResponse?>>
{
    public async Task<OperationResult<GetSectionTypeResponse?>> Handle(GetSectionTypeRequest request, CancellationToken cancellationToken)
    {
        var model = await repository.GetWithTranslationsAsNoTrackingAsync(request.Id, cancellationToken);
        if (model is null)
            return ErrorModel.Create("InvalidId");

        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var name = TranslationResolver.ResolveName(
            model.Translations,
            languageId,
            defaultLanguage.Id);

        return mapper.Map(model, name);
    }
}

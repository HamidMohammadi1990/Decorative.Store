using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Sections.Queries;

public class GetSectionHandler
    (ISectionRepository repository,
     ISectionMapperService mapper,
     ICurrentLanguageContext languageContext,
     ILanguageRegistry languageRegistry)
    : IRequestHandler<GetSectionRequest, OperationResult<GetSectionResponse?>>
{
    public async Task<OperationResult<GetSectionResponse?>> Handle(GetSectionRequest request, CancellationToken cancellationToken)
    {
        var model = await repository.GetWithTranslationsAsNoTrackingAsync(request.Id, cancellationToken);
        if (model is null)
            return ErrorModel.Create("InvalidId");

        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var (title, description, url) = TranslationResolver.Resolve(
            model.Translations,
            languageId,
            defaultLanguage.Id);

        return mapper.Map(model, title, description, url);
    }
}

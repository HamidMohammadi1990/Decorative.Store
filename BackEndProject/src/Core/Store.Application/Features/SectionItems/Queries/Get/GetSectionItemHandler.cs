using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionItems.Queries;

public class GetSectionItemHandler
    (ISectionItemRepository repository,
     ISectionItemMapperService mapper,
     ICurrentLanguageContext languageContext,
     ILanguageRegistry languageRegistry)
    : IRequestHandler<GetSectionItemRequest, OperationResult<GetSectionItemResponse?>>
{
    public async Task<OperationResult<GetSectionItemResponse?>> Handle(GetSectionItemRequest request, CancellationToken cancellationToken)
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

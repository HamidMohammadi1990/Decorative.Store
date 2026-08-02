using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductFiles.Queries;

public class GetProductFileHandler
     (IProductFileRepository productFileRepository,
      IProductFileMapperService mapper,
      ICurrentLanguageContext languageContext,
      ILanguageRegistry languageRegistry)
    : IRequestHandler<GetProductFileRequest, OperationResult<GetProductFileResponse?>>
{
    public async Task<OperationResult<GetProductFileResponse?>> Handle(GetProductFileRequest request, CancellationToken cancellationToken)
    {
        var productFile = await productFileRepository.GetWithTranslationsAsNoTrackingAsync(request.Id, cancellationToken);
        if (productFile is null)
            return ErrorModel.Create("InvalidId");

        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var title = ResolveTitle(productFile.Translations, languageId, defaultLanguage.Id);

        var result = mapper.Map(productFile, title);
        return result;
    }

    private static string ResolveTitle(
        IEnumerable<Store.Domain.Entities.ProductFileTranslation> translations,
        int languageId,
        int defaultLanguageId)
    {
        var list = translations as IList<Store.Domain.Entities.ProductFileTranslation> ?? translations.ToList();
        return list.FirstOrDefault(t => t.LanguageId == languageId)?.Title
            ?? list.FirstOrDefault(t => t.LanguageId == defaultLanguageId)?.Title
            ?? string.Empty;
    }
}

using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Products.Queries;

public class GetProductHandler
     (IProductRepository productRepository,
      IProductMapperService mapper,
      ICurrentLanguageContext languageContext,
      ILanguageRegistry languageRegistry)
     : IRequestHandler<GetProductRequest, OperationResult<GetProductResponse?>>
{
    public async Task<OperationResult<GetProductResponse?>> Handle(GetProductRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetWithTranslationsAsNoTrackingAsync(request.Id, cancellationToken);
        if (product is null)
            return ErrorModel.Create("InvalidId");

        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var (title, slug, description) = TranslationResolver.Resolve(product.Translations, languageId, defaultLanguage.Id);

        var result = mapper.Map(product, title, slug, description);
        return result;
    }
}

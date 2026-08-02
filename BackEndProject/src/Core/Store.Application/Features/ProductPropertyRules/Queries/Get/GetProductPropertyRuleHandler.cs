using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyRules.Queries;

public class GetProductPropertyRuleHandler
    (IProductPropertyRuleRepository productPropertyRuleRepository,
     IProductPropertyRuleMapperService mapper,
     ICurrentLanguageContext languageContext,
     ILanguageRegistry languageRegistry)
    : IRequestHandler<GetProductPropertyRuleRequest, OperationResult<GetProductPropertyRuleResponse?>>
{
    public async Task<OperationResult<GetProductPropertyRuleResponse?>> Handle(GetProductPropertyRuleRequest request, CancellationToken cancellationToken)
    {
        var productPropertyRule = await productPropertyRuleRepository.GetWithTranslationsAsNoTrackingAsync(request.Id, cancellationToken);
        if (productPropertyRule is null)
            return ErrorModel.Create("InvalidId");

        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var description = TranslationResolver.ResolveDescription(
            productPropertyRule.Translations,
            languageId,
            defaultLanguage.Id);

        return mapper.Map(productPropertyRule, description);
    }
}

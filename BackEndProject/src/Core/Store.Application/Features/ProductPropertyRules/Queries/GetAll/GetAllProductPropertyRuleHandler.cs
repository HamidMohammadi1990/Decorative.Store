using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyRules.Queries;

public class GetAllProductPropertyRuleHandler
    (IProductPropertyRuleRepository productPropertyRuleRepository,
     IProductPropertyRuleMapperService mapper,
     ICurrentLanguageContext languageContext,
     ILanguageRegistry languageRegistry)
    : IRequestHandler<GetAllProductPropertyRuleRequest, OperationResult<PagedResult<GetAllProductPropertyRuleResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllProductPropertyRuleResponse>>> Handle(GetAllProductPropertyRuleRequest request, CancellationToken cancellationToken)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;

        var requestModel = mapper.Map(request);
        var productPropertyRules = await productPropertyRuleRepository.GetAllAsync(requestModel);
        return mapper.Map(productPropertyRules, languageId, defaultLanguage.Id);
    }
}

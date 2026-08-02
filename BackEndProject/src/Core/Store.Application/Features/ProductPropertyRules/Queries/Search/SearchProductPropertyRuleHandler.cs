using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyRules.Queries;

public class SearchProductPropertyRuleHandler
    (IProductPropertyRuleRepository productPropertyRuleRepository,
     IProductPropertyRuleMapperService mapper,
     ICurrentLanguageContext languageContext,
     ILanguageRegistry languageRegistry)
    : IRequestHandler<SearchProductPropertyRuleRequest, OperationResult<PagedResult<SearchProductPropertyRuleResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchProductPropertyRuleResponse>>> Handle(SearchProductPropertyRuleRequest request, CancellationToken cancellationToken)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;

        var requestModel = mapper.Map(request);
        var productPropertyRules = await productPropertyRuleRepository.SearchAsync(requestModel);
        return mapper.MapToSearch(productPropertyRules, languageId, defaultLanguage.Id);
    }
}

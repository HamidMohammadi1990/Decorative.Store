using Store.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyRules.Queries;

public class SearchProductPropertyRuleHandler
    (IProductPropertyRuleRepository productPropertyRuleRepository, IProductPropertyRuleMapperService mapper)
    : IRequestHandler<SearchProductPropertyRuleRequest, OperationResult<PagedResult<SearchProductPropertyRuleResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchProductPropertyRuleResponse>>> Handle(SearchProductPropertyRuleRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var productPropertyRules = await productPropertyRuleRepository.SearchAsync(requestModel);
        return mapper.MapToSearch(productPropertyRules);
    }
}

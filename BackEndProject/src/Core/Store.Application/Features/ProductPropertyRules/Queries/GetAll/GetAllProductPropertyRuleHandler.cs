using Edition.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyRules.Queries;

public class GetAllProductPropertyRuleHandler
    (IProductPropertyRuleRepository productPropertyRuleRepository, IProductPropertyRuleMapperService mapper)
    : IRequestHandler<GetAllProductPropertyRuleRequest, OperationResult<PagedResult<GetAllProductPropertyRuleResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllProductPropertyRuleResponse>>> Handle(GetAllProductPropertyRuleRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var productPropertyRules = await productPropertyRuleRepository.GetAllAsync(requestModel);
        return mapper.Map(productPropertyRules);
    }
}

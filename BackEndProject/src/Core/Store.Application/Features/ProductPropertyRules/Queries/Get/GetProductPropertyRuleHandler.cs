using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyRules.Queries;

public class GetProductPropertyRuleHandler
    (IProductPropertyRuleRepository productPropertyRuleRepository, IProductPropertyRuleMapperService mapper)
    : IRequestHandler<GetProductPropertyRuleRequest, OperationResult<GetProductPropertyRuleResponse?>>
{
    public async Task<OperationResult<GetProductPropertyRuleResponse?>> Handle(GetProductPropertyRuleRequest request, CancellationToken cancellationToken)
    {
        var productPropertyRule = await productPropertyRuleRepository.GetAsNoTrackingAsync(request.Id);
        if (productPropertyRule is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(productPropertyRule);
    }
}

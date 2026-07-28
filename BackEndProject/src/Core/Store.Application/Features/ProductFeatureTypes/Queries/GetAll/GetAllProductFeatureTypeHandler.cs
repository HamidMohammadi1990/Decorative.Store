using Edition.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductFeatureTypes.Queries;

public class GetAllProductFeatureTypeHandler
    (IProductFeatureTypeRepository productFeatureTypeRepository, IProductFeatureTypeMapperService mapper)
    : IRequestHandler<GetAllProductFeatureTypeRequest, OperationResult<PagedResult<GetAllProductFeatureTypeResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllProductFeatureTypeResponse>>> Handle(GetAllProductFeatureTypeRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var productFeatureTypes = await productFeatureTypeRepository.GetAllAsync(requestModel);
        return mapper.Map(productFeatureTypes);
    }
}

using Edition.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductFeatureTypes.Queries;

public class SearchProductFeatureTypeHandler
    (IProductFeatureTypeRepository productFeatureTypeRepository, IProductFeatureTypeMapperService mapper)
    : IRequestHandler<SearchProductFeatureTypeRequest, OperationResult<PagedResult<SearchProductFeatureTypeResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchProductFeatureTypeResponse>>> Handle(SearchProductFeatureTypeRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var productFeatureTypes = await productFeatureTypeRepository.SearchAsync(requestModel);
        return mapper.MapToSearch(productFeatureTypes);
    }
}

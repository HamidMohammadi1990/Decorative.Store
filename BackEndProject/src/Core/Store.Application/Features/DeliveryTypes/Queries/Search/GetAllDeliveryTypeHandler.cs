using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.DeliveryTypes.Queries;

public class SearchDeliveryTypeHandler
    (IDeliveryTypeRepository deliveryTypeRepository, IDeliveryTypeMapperService mapper)
    : IRequestHandler<SearchDeliveryTypeRequest, OperationResult<PagedResult<SearchDeliveryTypeResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchDeliveryTypeResponse>>> Handle(SearchDeliveryTypeRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var deliveryTypes = await deliveryTypeRepository.SearchAsync(requestModel);
        var result = mapper.Map(deliveryTypes);
        return result;
    }
}
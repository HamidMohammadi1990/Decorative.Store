using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.DeliveryOptions.Queries;

public class SearchDeliveryOptionHandler 
    (IDeliveryOptionRepository deliveryOptionRepository, IDeliveryOptionMapperService mapper)
    : IRequestHandler<SearchDeliveryOptionRequest, OperationResult<PagedResult<SearchDeliveryOptionResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchDeliveryOptionResponse>>> Handle(SearchDeliveryOptionRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var deliveryOptions = await deliveryOptionRepository.SearchAsync(requestModel);
        var result = mapper.Map(deliveryOptions);
        return result;
    }
}
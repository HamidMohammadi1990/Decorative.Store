using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.DeliveryOptions.Queries;

public class GetAllDeliveryOptionHandler 
    (IDeliveryOptionRepository deliveryOptionRepository, IDeliveryOptionMapperService mapper)
    : IRequestHandler<GetAllDeliveryOptionRequest, OperationResult<PagedResult<GetAllDeliveryOptionResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllDeliveryOptionResponse>>> Handle(GetAllDeliveryOptionRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var deliveryOptions = await deliveryOptionRepository.GetAllAsync(requestModel);
        var result = mapper.Map(deliveryOptions);
        return result;
    }
}
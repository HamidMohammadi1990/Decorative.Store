using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.DeliveryTypes.Queries;

public class GetAllDeliveryTypeHandler
    (IDeliveryTypeRepository deliveryTypeRepository, IDeliveryTypeMapperService mapper)
    : IRequestHandler<GetAllDeliveryTypeRequest, OperationResult<PagedResult<GetAllDeliveryTypeResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllDeliveryTypeResponse>>> Handle(GetAllDeliveryTypeRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var deliveryTypes = await deliveryTypeRepository.GetAllAsync(requestModel);
        var result = mapper.Map(deliveryTypes);
        return result;
    }
}
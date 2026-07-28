using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.DeliveryTypes.Queries;

public class GetDeliveryTypeHandler
    (IDeliveryTypeRepository deliveryTypeRepository, IDeliveryTypeMapperService mapper)
    : IRequestHandler<GetDeliveryTypeRequest, OperationResult<GetDeliveryTypeResponse?>>
{
    public async Task<OperationResult<GetDeliveryTypeResponse?>> Handle(GetDeliveryTypeRequest request, CancellationToken cancellationToken)
    {
        var deliveryType = await deliveryTypeRepository.GetAsNoTrackingAsync(request.Id);
        if (deliveryType is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(deliveryType);
        return result;
    }
}
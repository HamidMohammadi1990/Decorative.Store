using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.DeliveryOptions.Queries;

public class GetDeliveryOptionHandler 
    (IDeliveryOptionRepository deliveryOptionRepository, IDeliveryOptionMapperService mapper)
    : IRequestHandler<GetDeliveryOptionRequest, OperationResult<GetDeliveryOptionResponse>>
{
    public async Task<OperationResult<GetDeliveryOptionResponse>> Handle(GetDeliveryOptionRequest request, CancellationToken cancellationToken)
    {
        var deliveryOption = await deliveryOptionRepository.GetAsNoTrackingAsync(request.Id);
        if (deliveryOption is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(deliveryOption);
        return result;
    }
}
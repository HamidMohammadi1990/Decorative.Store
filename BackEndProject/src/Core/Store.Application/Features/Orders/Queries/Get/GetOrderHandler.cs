using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Orders.Queries.Get;

public class GetOrderHandler
    (IOrderRepository orderRepository, IOrderMapperService mapper)
    : IRequestHandler<GetOrderRequest, OperationResult<GetOrderResponse?>>
{
    public async Task<OperationResult<GetOrderResponse?>> Handle(GetOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsNoTrackingAsync(request.Id);
        if (order is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(order);
        return result;
    }
}
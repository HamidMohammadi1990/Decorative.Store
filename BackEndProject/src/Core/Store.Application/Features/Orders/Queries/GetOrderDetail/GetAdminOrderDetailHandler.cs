using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Orders.Queries;

public class GetAdminOrderDetailHandler
    (IOrderRepository orderRepository, IOrderMapperService orderMapperService)
    : IRequestHandler<GetAdminOrderDetailRequest, OperationResult<GetOrderDetailResponse?>>
{
    public async Task<OperationResult<GetOrderDetailResponse?>> Handle(
        GetAdminOrderDetailRequest request,
        CancellationToken cancellationToken)
    {
        var orderDetails = await orderRepository.GetOrderDetailAsync(request.OrderId, userId: null, cancellationToken);
        var response = orderMapperService.MapToOrderDetail(orderDetails);
        return response;
    }
}

using Edition.Application.Contracts;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Orders.Queries;

public class GetOrderByStatusHandler
    (IOrderRepository orderRepository, ICurrentUserContext currentUser, IOrderMapperService orderMapperService)
     : IRequestHandler<GetOrderByStatusRequest, OperationResult<List<GetOrderByStatusResponse>>>
{
    public async Task<OperationResult<List<GetOrderByStatusResponse>>> Handle(GetOrderByStatusRequest request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var order = await orderRepository.GetUserOrdersByStatusAsync(userId, request.Status, request.Pagination);
        var result = orderMapperService.MapToUserOrdersByStatus(order);
        return result;
    }
}
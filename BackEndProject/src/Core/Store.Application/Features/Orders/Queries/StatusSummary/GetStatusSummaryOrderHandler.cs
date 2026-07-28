using Edition.Application.Contracts;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Orders.Queries
{
    public class GetStatusSummaryOrderHandler
        (IOrderRepository orderRepository, ICurrentUserContext currentUser, IOrderMapperService orderMapperService)
        : IRequestHandler<GetStatusSummaryOrderRequest, OperationResult<List<GetStatusSummaryOrderResponse>>>
    {
        public async Task<OperationResult<List<GetStatusSummaryOrderResponse>>> Handle(GetStatusSummaryOrderRequest request, CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId;
            var orders = await orderRepository.GetUserOrderStatusSummaryAsync(userId);
            return orderMapperService.MapToStatusSummary(orders);
        }
    }
}
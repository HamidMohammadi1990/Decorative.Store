using Edition.Application.Contracts;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Orders.Queries;

public class GetOrderDetailHandler
	(IOrderRepository orderRepository, ICurrentUserContext currentUser, IOrderMapperService orderMapperService)
	: IRequestHandler<GetOrderDetailRequest, OperationResult<GetOrderDetailResponse?>>
{
	public async Task<OperationResult<GetOrderDetailResponse?>> Handle(GetOrderDetailRequest request, CancellationToken cancellationToken)
	{
		var userId = currentUser.UserId;
		var orderDetails = await orderRepository.GetOrderDetailAsync(request.OrderId, userId, cancellationToken);
		var response = orderMapperService.MapToOrderDetail(orderDetails);
		return response;
	}
}
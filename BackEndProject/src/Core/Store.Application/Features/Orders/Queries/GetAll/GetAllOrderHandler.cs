using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Orders.Queries;

public class GetAllOrderHandler
    (IOrderRepository orderRepository, IOrderMapperService mapper)
    : IRequestHandler<GetAllOrderRequest, OperationResult<PagedResult<GetAllOrderResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllOrderResponse>>> Handle(GetAllOrderRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var orders = await orderRepository.GetAllAsync(requestModel);
        var result = mapper.Map(orders);
        return result;
    }
}
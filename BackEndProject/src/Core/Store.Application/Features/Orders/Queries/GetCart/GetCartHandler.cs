using Edition.Application.Contracts;
using Edition.Application.Features.Orders.Commands;
using Edition.Application.Features.Orders.Common;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Orders.Queries;

public record GetCartRequest : IRequest<OperationResult<GetCartResponse>>;

public class GetCartHandler
    (
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IDiscountRepository discountRepository,
        ICurrentUserContext currentUser)
    : IRequestHandler<GetCartRequest, OperationResult<GetCartResponse>>
{
    public async Task<OperationResult<GetCartResponse>> Handle(
        GetCartRequest request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var userIsCooperation = currentUser.IsCooperation;
        var order = await orderRepository.GetPendingOrderByUserIdAsync(userId);

        if (order is null)
        {
            return new GetCartResponse
            {
                Summary = OrderCartSummaryMapper.Map(
                    CartModificationResult.WithoutDiscount(0))
            };
        }

        var cartResult = await OrderCartService.RefreshCartAsync(
            order, discountRepository, userIsCooperation, cancellationToken);

        return await CartResponseMapper.MapAsync(
            productRepository, order, cartResult, cancellationToken);
    }
}

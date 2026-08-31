using Store.Common.Extensions;
using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Features.Orders.Common;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Commands;

public class UpdateCartItemQuantityHandler
    : IRequestHandler<UpdateCartItemQuantityRequest, OperationResult<GetCartResponse>>
{
    private readonly IUnitOfWork uow;
    private readonly IOrderRepository orderRepository;
    private readonly IProductRepository productRepository;
    private readonly IDiscountRepository discountRepository;
    private readonly ICurrentUserContext currentUser;

    public UpdateCartItemQuantityHandler(
        IUnitOfWork uow,
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IDiscountRepository discountRepository,
        ICurrentUserContext currentUser)
    {
        this.uow = uow;
        this.orderRepository = orderRepository;
        this.productRepository = productRepository;
        this.discountRepository = discountRepository;
        this.currentUser = currentUser;
    }

    public async Task<OperationResult<GetCartResponse>> Handle(
        UpdateCartItemQuantityRequest request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var userIsCooperation = currentUser.IsCooperation;
        var order = await orderRepository.GetByUserIdAsync(userId, OrderStatusType.Pending);
        if (order is null)
            return ErrorModel.Create("OrderIsNotFound");

        if (await orderRepository.HasPendingBankPaymentAsync(order.Id, cancellationToken))
            return ErrorModel.Create("PaymentInProgress");

        var orderItem = order.OrderItems.SingleOrDefault(x => x.Id == request.OrderItemId);
        if (orderItem is null)
            return ErrorModel.Create("OrderItemNotFound");

        orderItem.SetQuantity(request.Quantity);

        var cartResult = await OrderCartService.RefreshCartAsync(
            order, discountRepository, userIsCooperation, cancellationToken);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<GetCartResponse>();

        return await CartResponseMapper.MapAsync(productRepository, order, cartResult, cancellationToken);
    }
}

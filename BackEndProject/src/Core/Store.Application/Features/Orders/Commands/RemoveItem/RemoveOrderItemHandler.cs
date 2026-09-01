using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Features.Orders.Common;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Commands;

public class RemoveOrderItemHandler
    : IRequestHandler<RemoveOrderItemRequest, OperationResult<RemoveOrderItemResponse>>
{
    private readonly IUnitOfWork uow;
    private readonly IOrderRepository orderRepository;
    private readonly IDiscountRepository discountRepository;
    private readonly ICurrentUserContext currentUser;

    public RemoveOrderItemHandler(
        IUnitOfWork uow,
        IOrderRepository orderRepository,
        IDiscountRepository discountRepository,
        ICurrentUserContext currentUser)
    {
        this.uow = uow;
        this.orderRepository = orderRepository;
        this.discountRepository = discountRepository;
        this.currentUser = currentUser;
    }

    public async Task<OperationResult<RemoveOrderItemResponse>> Handle(
        RemoveOrderItemRequest request,
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

        orderRepository.RemoveOrderItem(order, orderItem);

        var cartResult = await OrderCartService.RefreshCartAsync(
            order, discountRepository, userIsCooperation, cancellationToken);

        var isOrderDeleted = false;
        int? orderId = order.Id;
        long? trackingCode = order.TrackingCode;

        if (order.CanBeDeletedAsEmptyCart())
        {
            if (await orderRepository.HasFinancialDocumentsAsync(order.Id, cancellationToken))
                return ErrorModel.Create("OrderCannotBeDeleted");

            orderRepository.Remove(order);
            isOrderDeleted = true;
            orderId = null;
            trackingCode = null;
        }

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<RemoveOrderItemResponse>();

        return new RemoveOrderItemResponse
        {
            OrderId = orderId,
            TrackingCode = trackingCode,
            IsOrderDeleted = isOrderDeleted,
            Cart = OrderCartSummaryMapper.Map(cartResult)
        };
    }
}

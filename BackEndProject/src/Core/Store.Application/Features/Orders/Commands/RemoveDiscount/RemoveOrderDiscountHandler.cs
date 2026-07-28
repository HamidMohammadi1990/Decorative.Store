using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Features.Orders.Common;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Orders;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Commands;

public class RemoveOrderDiscountHandler
    : IRequestHandler<RemoveOrderDiscountRequest, OperationResult<RemoveOrderDiscountResponse>>
{
    private readonly IUnitOfWork uow;
    private readonly IOrderRepository orderRepository;
    private readonly IDiscountRepository discountRepository;
    private readonly ICurrentUserContext currentUser;

    public RemoveOrderDiscountHandler(
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

    public async Task<OperationResult<RemoveOrderDiscountResponse>> Handle(
        RemoveOrderDiscountRequest request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var order = await orderRepository.GetByUserIdAsync(userId, OrderStatusType.Pending);
        if (order is null)
            return ErrorModel.Create("OrderIsNotFound");

        if (await orderRepository.HasPendingBankPaymentAsync(order.Id, cancellationToken))
            return ErrorModel.Create("PaymentInProgress");

        if (order.AppliedDiscountId.HasValue && order.IsDiscountUsageConsumed)
        {
            var discount = await discountRepository.GetByIdAsync(order.AppliedDiscountId.Value);
            if (discount is not null)
                order.ReleaseDiscountUsage(discount);
        }

        order.RecalculateTotalPrice();
        order.ClearDiscountApplication();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<RemoveOrderDiscountResponse>();

        return new RemoveOrderDiscountResponse
        {
            OrderId = order.Id,
            Cart = OrderCartSummaryMapper.Map(CartModificationResult.WithoutDiscount(order.TotalPrice))
        };
    }
}

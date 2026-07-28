using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Features.Orders.Common;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Queries;

public class ValidateDiscountOrderHandler
    : IRequestHandler<ValidateDiscountOrderRequest, OperationResult<ValidateDiscountOrderResponse>>
{
    private readonly IUnitOfWork uow;
    private readonly IOrderRepository orderRepository;
    private readonly IDiscountRepository discountRepository;
    private readonly ICurrentUserContext currentUser;

    public ValidateDiscountOrderHandler(
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

    public async Task<OperationResult<ValidateDiscountOrderResponse>> Handle(
        ValidateDiscountOrderRequest request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var userIsCooperation = currentUser.IsCooperation;
        var order = await orderRepository.GetByUserIdAsync(userId, OrderStatusType.Pending);
        if (order is null)
            return ErrorModel.Create("OrderIsNotFound");

        if (await orderRepository.HasPendingBankPaymentAsync(order.Id, cancellationToken))
            return ErrorModel.Create("PaymentInProgress");

        order.RecalculateTotalPrice();

        var discount = await discountRepository.GetByCodeAsync(request.DiscountCode.Trim());
        if (discount is null)
            return ErrorModel.Create("DiscountNotFound");

        if (order.AppliedDiscountId.HasValue &&
            order.AppliedDiscountId != discount.Id &&
            order.IsDiscountUsageConsumed)
        {
            var previousDiscount = await discountRepository.GetByIdAsync(order.AppliedDiscountId.Value);
            if (previousDiscount is not null)
                order.ReleaseDiscountUsage(previousDiscount);
        }

        var result = order.EvaluateDiscount(discount, userIsCooperation);
        if (!result.IsSuccess)
            return DiscountFailureMapper.ToErrorModel(result.Failure!.Value);

        order.SetDiscountPreview(discount.Id, result.DiscountAmount);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<ValidateDiscountOrderResponse>();

        return new ValidateDiscountOrderResponse
        {
            DiscountCode = discount.Code,
            TotalPrice = order.TotalPrice,
            DiscountAmount = result.DiscountAmount,
            FinalPrice = order.FinalPrice
        };
    }
}

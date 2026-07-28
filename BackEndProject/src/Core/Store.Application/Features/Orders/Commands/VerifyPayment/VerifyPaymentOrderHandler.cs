using Edition.Application.Contracts;
using Edition.Application.Contracts.Orders;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Features.Orders.Common;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Commands;

public class VerifyPaymentOrderHandler
    : IRequestHandler<VerifyPaymentOrderRequest, OperationResult<VerifyPaymentOrderResponse>>
{
    private readonly IUnitOfWork uow;
    private readonly IOrderRepository orderRepository;
    private readonly IBankTransactionRepository bankTransactionRepository;
    private readonly IDiscountRepository discountRepository;
    private readonly ICurrentUserContext currentUser;
    private readonly IBankPaymentVerificationService bankPaymentVerificationService;

    public VerifyPaymentOrderHandler(
        IUnitOfWork uow,
        IOrderRepository orderRepository,
        IBankTransactionRepository bankTransactionRepository,
        IDiscountRepository discountRepository,
        ICurrentUserContext currentUser,
        IBankPaymentVerificationService bankPaymentVerificationService)
    {
        this.uow = uow;
        this.orderRepository = orderRepository;
        this.bankTransactionRepository = bankTransactionRepository;
        this.discountRepository = discountRepository;
        this.currentUser = currentUser;
        this.bankPaymentVerificationService = bankPaymentVerificationService;
    }

    public async Task<OperationResult<VerifyPaymentOrderResponse>> Handle(
        VerifyPaymentOrderRequest request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var bankTransaction = await bankTransactionRepository.GetLatestForVerificationAsync(
            userId,
            request.BankTransactionId,
            cancellationToken);

        if (bankTransaction is null)
        {
            if (request.BankTransactionId.HasValue)
                return ErrorModel.Create("BankTransactionNotFound");

            return await BuildWalletOnlyCompletedResponseAsync(userId, cancellationToken);
        }

        var order = bankTransaction.FinancialDocument.Order;

        if (bankTransaction.Status == TransactionStatusType.Completed)
            return BuildResponse(order, bankTransaction, isPaymentSuccessful: true);

        if (bankTransaction.Status is TransactionStatusType.Failed or TransactionStatusType.Canceled)
            return ErrorModel.Create("PaymentFailed");

        var pendingPayment = await bankTransactionRepository.GetPendingPaymentAsync(
            userId,
            bankTransaction.Id,
            cancellationToken);

        if (pendingPayment is null)
            return ErrorModel.Create("NoPendingBankPayment");

        var isVerified = await bankPaymentVerificationService.VerifyAsync(
            pendingPayment.BankTransaction,
            pendingPayment.BankTransaction.BankAccount,
            request.GatewayReference,
            cancellationToken);

        Discount? appliedDiscount = null;
        if (pendingPayment.Order.AppliedDiscountId.HasValue)
            appliedDiscount = await discountRepository.GetByIdAsync(pendingPayment.Order.AppliedDiscountId.Value);

        if (isVerified)
        {
            OrderPaymentSettlement.CompleteSuccessfulBankPayment(
                pendingPayment.Order,
                pendingPayment.BankTransaction,
                pendingPayment.PendingOrderVats,
                request.GatewayReference);
        }
        else
        {
            OrderPaymentSettlement.FailBankPayment(
                pendingPayment.Order,
                pendingPayment.BankTransaction,
                pendingPayment.WalletTransactions,
                appliedDiscount,
                userId);
        }

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<VerifyPaymentOrderResponse>();

        if (!isVerified)
            return ErrorModel.Create("PaymentFailed");

        return BuildResponse(pendingPayment.Order, pendingPayment.BankTransaction, isPaymentSuccessful: true);
    }

    private async Task<OperationResult<VerifyPaymentOrderResponse>> BuildWalletOnlyCompletedResponseAsync(
        int userId,
        CancellationToken cancellationToken)
    {
        var completedOrder = await orderRepository.GetByUserIdAsync(userId, OrderStatusType.InProgress);
        if (completedOrder is not null && completedOrder.IsFinaly)
        {
            return new VerifyPaymentOrderResponse
            {
                IsPaymentSuccessful = true,
                TrackingCode = completedOrder.TrackingCode,
                Status = completedOrder.Status,
                FinalPrice = completedOrder.FinalPrice,
                WalletDeduction = completedOrder.FinalPrice,
                BankPaymentAmount = 0
            };
        }

        return ErrorModel.Create("NoPendingBankPayment");
    }

    private static VerifyPaymentOrderResponse BuildResponse(
        Order order,
        BankTransaction bankTransaction,
        bool isPaymentSuccessful)
    {
        var walletDeduction = order.FinalPrice - bankTransaction.Amount;
        if (walletDeduction < 0)
            walletDeduction = 0;

        return new VerifyPaymentOrderResponse
        {
            IsPaymentSuccessful = isPaymentSuccessful,
            TrackingCode = order.TrackingCode,
            Status = order.Status,
            FinalPrice = order.FinalPrice,
            WalletDeduction = walletDeduction,
            BankPaymentAmount = bankTransaction.Amount
        };
    }
}

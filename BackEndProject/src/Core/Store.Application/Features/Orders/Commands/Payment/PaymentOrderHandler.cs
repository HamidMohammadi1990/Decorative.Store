using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Features.Orders.Common;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Orders.Commands;

public class PaymentOrderHandler
    : IRequestHandler<PaymentOrderRequest, OperationResult<PaymentOrderResponse>>
{
    private readonly IUnitOfWork uow;
    private readonly IOrderRepository orderRepository;
    private readonly IWalletRepository walletRepository;
    private readonly IBankAccountRepository bankAccountRepository;
    private readonly IDiscountRepository discountRepository;
    private readonly ICurrentUserContext currentUser;
    private readonly IFinancialYearRepository financialYearRepository;
    private readonly IFinancialDocumentRepository financialDocumentRepository;

    public PaymentOrderHandler(
        IUnitOfWork uow,
        IOrderRepository orderRepository,
        IWalletRepository walletRepository,
        IBankAccountRepository bankAccountRepository,
        IDiscountRepository discountRepository,
        ICurrentUserContext currentUser,
        IFinancialYearRepository financialYearRepository,
        IFinancialDocumentRepository financialDocumentRepository)
    {
        this.uow = uow;
        this.orderRepository = orderRepository;
        this.walletRepository = walletRepository;
        this.bankAccountRepository = bankAccountRepository;
        this.discountRepository = discountRepository;
        this.currentUser = currentUser;
        this.financialYearRepository = financialYearRepository;
        this.financialDocumentRepository = financialDocumentRepository;
    }

    public async Task<OperationResult<PaymentOrderResponse>> Handle(
        PaymentOrderRequest request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var userIsCooperation = currentUser.IsCooperation;
        var order = await orderRepository.GetByUserIdAsync(userId, OrderStatusType.Pending);
        if (order is null)
            return ErrorModel.Create("OrderIsNotFound");

        if (order.OrderItems.Count == 0)
            return ErrorModel.Create("OrderIsEmpty");

        if (await orderRepository.HasPendingBankPaymentAsync(order.Id, cancellationToken))
            return ErrorModel.Create("PaymentInProgress");

        order.RecalculateTotalPrice();

        Discount? appliedDiscount = null;
        if (!string.IsNullOrWhiteSpace(request.DiscountCode))
        {
            appliedDiscount = await discountRepository.GetByCodeAsync(request.DiscountCode.Trim());
            if (appliedDiscount is null)
                return ErrorModel.Create("DiscountNotFound");
        }
        else if (order.AppliedDiscountId.HasValue)
        {
            appliedDiscount = await discountRepository.GetByIdAsync(order.AppliedDiscountId.Value);
        }

        if (appliedDiscount is not null)
        {
            var discountResult = order.TryApplyDiscount(appliedDiscount, userIsCooperation);
            if (!discountResult.IsSuccess)
                return DiscountFailureMapper.ToErrorModel(discountResult.Failure!.Value);
        }
        else
        {
            if (order.IsDiscountUsageConsumed)
                return ErrorModel.Create("DiscountAlreadyConsumed");

            order.ClearDiscountApplication();
        }

        order.RecalculateVatPrice(OrderPaymentConstants.VatRate);

        if (order.FinalPrice <= 0)
            return ErrorModel.Create("OrderIsEmpty");

        var paymentSlice = OrderPaymentAllocator.Allocate(order);
        if (paymentSlice is null)
            return ErrorModel.Create("OrderIsEmpty");

        var financialYear = await financialYearRepository.GetFirstActiveAsync(cancellationToken);
        if (financialYear is null)
            return ErrorModel.Create("FinancialYearNotFound");

        Wallet? wallet = null;
        if (request.PaymentOption is PaymentOptionType.WalletOnly or PaymentOptionType.WalletAndBank)
        {
            wallet = await walletRepository.FindByUserIdAsync(userId, request.WalletId!.Value);
            if (wallet is null)
                return ErrorModel.Create("WalletNotFound");

            if (wallet.Status != WalletStatusType.Active)
                return ErrorModel.Create("WalletIsNotActive");
        }

        BankAccount? bankAccount = null;
        var paymentAmounts = OrderPaymentAmounts.Calculate(
            request.PaymentOption,
            order.FinalPrice,
            wallet?.Balance ?? 0m);

        if (request.PaymentOption == PaymentOptionType.WalletOnly &&
            wallet!.Balance < order.FinalPrice)
            return ErrorModel.Create("InsufficientWalletBalance");

        if (paymentAmounts.BankPaymentAmount > 0)
        {
            bankAccount = await bankAccountRepository.GetActiveByIdAsync(request.BankId!.Value, cancellationToken);
            if (bankAccount is null)
                return ErrorModel.Create("InvalidBankId");
        }

        var isFullyPaid = paymentAmounts.BankPaymentAmount == 0;
        var trackingDescription = $"سفارش با کد پیگیری {order.TrackingCode}";

        var financialDocument = OrderPaymentFinancialDocumentBuilder.Build(
            order,
            paymentSlice,
            financialYear.Id,
            isFullyPaid);

        if (paymentAmounts.WalletDeduction > 0)
        {
            wallet!.DecreaseBalance(paymentAmounts.WalletDeduction);

            var walletTransaction = WalletTransaction.CreateDecremental(
                wallet.Id,
                paymentAmounts.WalletDeduction,
                $"پرداخت از کیف پول بابت {trackingDescription}",
                WalletTransactionStatusType.Completed,
                userId);

            financialDocument.AddWalletTransaction(walletTransaction);
        }

        BankTransaction? bankTransaction = null;
        if (paymentAmounts.BankPaymentAmount > 0)
        {
            bankTransaction = BankTransaction.Create(
                TransactionStatusType.Pending,
                bankAccount!.Id,
                userId,
                paymentAmounts.BankPaymentAmount,
                string.Empty,
                $"پرداخت بابت {trackingDescription}");

            financialDocument.AddBankTransaction(bankTransaction);
        }

        financialDocumentRepository.Add(financialDocument);

        if (appliedDiscount is not null)
            order.ConsumeDiscountUsage(appliedDiscount);

        if (isFullyPaid)
            order.CompletePayment();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<PaymentOrderResponse>();

        return new PaymentOrderResponse
        {
            BankTransactionId = bankTransaction?.Id,
            PaymentUrl = paymentAmounts.BankPaymentAmount > 0 ? bankAccount!.PaymentUrl : null,
            IsBankPaymentRequired = paymentAmounts.BankPaymentAmount > 0,
            BankPaymentAmount = paymentAmounts.BankPaymentAmount,
            WalletDeduction = paymentAmounts.WalletDeduction,
            TotalPrice = order.TotalPrice,
            DiscountAmount = order.DiscountAmount,
            FinalPrice = order.FinalPrice
        };
    }
}

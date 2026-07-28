using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Common;

public static class OrderPaymentSettlement
{
    public static void CompleteSuccessfulBankPayment(
        Order order,
        BankTransaction bankTransaction,
        IEnumerable<OrderVat> pendingOrderVats,
        string? gatewayReference)
    {
        var transactionNumber = string.IsNullOrWhiteSpace(gatewayReference)
            ? bankTransaction.TransactionNumber
            : gatewayReference.Trim();

        if (string.IsNullOrWhiteSpace(transactionNumber))
            transactionNumber = $"BANK-{bankTransaction.Id}";

        bankTransaction.MarkAsCompleted(transactionNumber);

        foreach (var orderVat in pendingOrderVats)
            orderVat.MarkAsPaid();

        if (order.Status == OrderStatusType.Pending)
            order.CompletePayment();
    }

    public static void FailBankPayment(
        Order order,
        BankTransaction bankTransaction,
        IEnumerable<WalletTransaction> walletTransactions,
        Discount? appliedDiscount,
        int userId)
    {
        bankTransaction.MarkAsFailed();

        foreach (var walletTransaction in walletTransactions)
        {
            walletTransaction.Wallet.IncreaseBalance(walletTransaction.Amount);

            var refundTransaction = WalletTransaction.CreateIncremental(
                walletTransaction.WalletId,
                walletTransaction.Amount,
                $"عودت وجه به دلیل عدم موفقیت پرداخت بانکی - {walletTransaction.Description}",
                WalletTransactionStatusType.Completed,
                userId);

            walletTransaction.FinancialDocument.AddWalletTransaction(refundTransaction);
        }

        if (appliedDiscount is not null && order.IsDiscountUsageConsumed)
            order.ReleaseDiscountUsage(appliedDiscount);
    }
}

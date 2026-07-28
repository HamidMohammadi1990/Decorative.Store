using Store.Domain.Dtos.Wallets;

namespace Edition.Application.Features.Wallets.Common;

public static class WalletChargeSettlement
{
    public static void CompleteSuccessfulBankCharge(WalletPendingBankCharge pending, string? gatewayReference)
    {
        pending.BankTransaction.MarkAsCompleted(
            gatewayReference ?? pending.BankTransaction.TransactionNumber);

        pending.WalletTransaction.MarkAsCompleted();
        pending.Wallet.IncreaseBalance(pending.WalletTransaction.Amount);
    }

    public static void FailBankCharge(WalletPendingBankCharge pending)
    {
        pending.BankTransaction.MarkAsFailed();
        pending.WalletTransaction.MarkAsFailed();
    }
}

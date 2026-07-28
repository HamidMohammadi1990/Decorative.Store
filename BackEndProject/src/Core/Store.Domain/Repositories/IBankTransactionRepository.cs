using Store.Domain.Dtos.Orders;
using Store.Domain.Dtos.Wallets;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IBankTransactionRepository
{
    Task<BankTransaction?> GetLatestForVerificationAsync(
        int userId,
        int? bankTransactionId,
        CancellationToken cancellationToken = default);

    Task<OrderPendingBankPayment?> GetPendingPaymentAsync(
        int userId,
        int? bankTransactionId,
        CancellationToken cancellationToken = default);

    Task<WalletPendingBankCharge?> GetPendingWalletChargeAsync(
        int userId,
        int? bankTransactionId,
        CancellationToken cancellationToken = default);

    Task<WalletPendingBankCharge?> GetWalletChargeAsync(
        int userId,
        int? bankTransactionId,
        CancellationToken cancellationToken = default);
}

using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence;
using Store.Domain.Enums;
using Store.Domain.Dtos.Wallets;
using Store.Domain.Dtos.Orders;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class BankTransactionRepository
    (EditionDbContext context)
    : Repository<BankTransaction>(context), IBankTransactionRepository
{
    public Task<BankTransaction?> GetLatestForVerificationAsync(
        int userId,
        int? bankTransactionId,
        CancellationToken cancellationToken = default)
    {
        var query = Context.BankTransaction
            .Include(x => x.BankAccount)
            .Include(x => x.FinancialDocument)
                .ThenInclude(fd => fd.Order)
            .Where(x => x.UserId == userId);

        if (bankTransactionId.HasValue)
            query = query.Where(x => x.Id == bankTransactionId.Value);

        return query
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<OrderPendingBankPayment?> GetPendingPaymentAsync(
        int userId,
        int? bankTransactionId,
        CancellationToken cancellationToken = default)
    {
        var bankTransaction = await GetLatestForVerificationAsync(userId, bankTransactionId, cancellationToken);
        if (bankTransaction is null || bankTransaction.Status != TransactionStatusType.Pending)
            return null;

        var orderId = bankTransaction.FinancialDocument.OrderId;
        if (!orderId.HasValue)
            return null;

        var walletTransactions = await Context.WalletTransaction
            .Include(walletTransaction => walletTransaction.Wallet)
            .Include(walletTransaction => walletTransaction.FinancialDocument)
            .Where(walletTransaction =>
                walletTransaction.FinancialDocument.OrderId == orderId.Value &&
                walletTransaction.Type == WalletTransactionType.Decremental &&
                walletTransaction.Status == WalletTransactionStatusType.Completed)
            .ToListAsync(cancellationToken);

        var pendingOrderVats = await Context.OrderVat
            .Where(orderVat =>
                orderVat.OrderId == orderId.Value &&
                orderVat.Status == OrderVatStatusType.Pending)
            .ToListAsync(cancellationToken);

        return new OrderPendingBankPayment
        {
            BankTransaction = bankTransaction,
            Order = bankTransaction.FinancialDocument.Order,
            WalletTransactions = walletTransactions,
            PendingOrderVats = pendingOrderVats
        };
    }

    public async Task<WalletPendingBankCharge?> GetPendingWalletChargeAsync(
        int userId,
        int? bankTransactionId,
        CancellationToken cancellationToken = default)
    {
        var charge = await GetWalletChargeAsync(userId, bankTransactionId, cancellationToken);
        if (charge is null ||
            charge.BankTransaction.Status != TransactionStatusType.Pending ||
            charge.WalletTransaction.Status != WalletTransactionStatusType.Pending)
            return null;

        return charge;
    }

    public async Task<WalletPendingBankCharge?> GetWalletChargeAsync(
        int userId,
        int? bankTransactionId,
        CancellationToken cancellationToken = default)
    {
        var bankTransaction = await GetLatestForVerificationAsync(userId, bankTransactionId, cancellationToken);
        if (bankTransaction is null || bankTransaction.FinancialDocument.OrderId.HasValue)
            return null;

        var walletTransaction = await Context.WalletTransaction
            .Include(x => x.Wallet)
            .Where(x =>
                x.FinancialDocumentId == bankTransaction.FinancialDocumentId &&
                x.Type == WalletTransactionType.Incremental)
            .SingleOrDefaultAsync(cancellationToken);

        if (walletTransaction is null)
            return null;

        return new WalletPendingBankCharge
        {
            BankTransaction = bankTransaction,
            WalletTransaction = walletTransaction,
            Wallet = walletTransaction.Wallet
        };
    }
}
using Store.Domain.Entities;

namespace Store.Domain.Dtos.Orders;

public sealed class OrderPendingBankPayment
{
    public required BankTransaction BankTransaction { get; init; }
    public required Order Order { get; init; }
    public IReadOnlyList<WalletTransaction> WalletTransactions { get; init; } = [];
    public IReadOnlyList<OrderVat> PendingOrderVats { get; init; } = [];
}

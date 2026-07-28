using Store.Domain.Entities;

namespace Store.Domain.Dtos.Wallets;

public sealed class WalletPendingBankCharge
{
    public BankTransaction BankTransaction { get; init; } = default!;
    public WalletTransaction WalletTransaction { get; init; } = default!;
    public Wallet Wallet { get; init; } = default!;
}

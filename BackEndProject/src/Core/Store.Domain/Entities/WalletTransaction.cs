using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class WalletTransaction : BaseEntity
{
    public int? UserId { get; private set; }
    public int FinancialDocumentId { get; set; } = default!;
    public int? DestinationWalletId { get; set; }
    public decimal Amount { get; private set; }
    public int WalletId { get; private set; }
    public string Description { get; private set; } = default!;
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public WalletTransactionStatusType Status { get; private set; }
    public WalletTransactionType Type { get; private set; }

    public User User { get; private set; } = default!;
    public Wallet Wallet { get; private set; } = default!;
    public Wallet DestinationWallet { get; private set; } = default!;
    public FinancialDocument FinancialDocument { get; private set; } = default!;
    public ICollection<Expense> Expenses { get; private set; } = default!;

    public static WalletTransaction Create(int walletId, decimal amount, string description, WalletTransactionStatusType status, WalletTransactionType type)
        => new()
        {
            Type = type,
            Amount = amount,
            Status = status,
            WalletId = walletId,
            Description = description,
        };

    public static WalletTransaction CreateIncremental(
        int walletId,
        decimal amount,
        string description,
        WalletTransactionStatusType status,
        int? userId = null)
       => new()
       {
           Type = WalletTransactionType.Incremental,
           Amount = amount,
           Status = status,
           UserId = userId,
           WalletId = walletId,
           Description = description,
       };

    public static WalletTransaction CreateDecremental(
        int walletId,
        decimal amount,
        string description,
        WalletTransactionStatusType status,
        int? userId = null)
       => new()
       {
           Type = WalletTransactionType.Decremental,
           Amount = amount,
           Status = status,
           UserId = userId,
           WalletId = walletId,
           Description = description,
       };

    public void MarkAsCompleted()
    {
        if (Status != WalletTransactionStatusType.Pending)
            throw new InvalidOperationException("Only pending wallet transactions can be completed.");

        Status = WalletTransactionStatusType.Completed;
    }

    public void MarkAsFailed()
    {
        if (Status != WalletTransactionStatusType.Pending)
            throw new InvalidOperationException("Only pending wallet transactions can be marked as failed.");

        Status = WalletTransactionStatusType.Failed;
    }
}

using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class Wallet : BaseEntity
{
    public string Title { get; private set; } = default!;
    public int? UserId { get; private set; }
    public int? CompanyId { get; private set; }
    public bool IsDefault { get; private set; }
    public decimal Balance { get; private set; }
    public WalletStatusType Status { get; private set; } = WalletStatusType.Active;
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;


    public User User { get; set; } = default!;
    public Company Company { get; set; } = default!;
    public ICollection<WalletTransaction> WalletTransactions { get; set; } = default!;
    public ICollection<WalletTransaction> DestinationWalletTransactions { get; set; } = default!;


    public static Wallet Create(int userId, string title, bool isDefault = false, int? companyId = null)
        => new()
        {
            Title = title,
            UserId = userId,
            IsDefault = isDefault,
            CompanyId = companyId
        };

    public void Update(string title, bool isDefault)
    {
        Title = title;
        IsDefault = isDefault;
    }

    public void ChangeStatus(WalletStatusType status)
    {
        Status = status;
    }

    public void SetDefault(bool isDefault)
    {
        IsDefault = isDefault;
    }

    public bool HasSufficientBalance(decimal amount)
        => Balance >= amount;

    public void IncreaseBalance(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        Balance += amount;
    }

    public void DecreaseBalance(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        if (!HasSufficientBalance(amount))
            throw new InvalidOperationException("Insufficient wallet balance.");

        Balance -= amount;
    }
}
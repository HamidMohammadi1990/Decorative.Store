using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class BankTransaction : BaseEntity
{
    public int? UserId { get; private set; }
    public int BankAccountId { get; private set; } = default!;
    public decimal Amount { get; private set; } = default!;
    public string TransactionNumber { get; private set; } = default!;
    public DateTime TransactionDateOnUtc { get; private set; } = DateTime.UtcNow;
    public int FinancialDocumentId { get; private set; } = default!;
    public TransactionStatusType Status { get; private set; } = TransactionStatusType.Pending;
    public string Description { get; private set; } = default!;

    public User User { get; private set; } = default!;
    public BankAccount BankAccount { get; private set; } = default!;
    public FinancialDocument FinancialDocument { get; private set; } = default!;
    public ICollection<Expense> Expenses { get; private set; } = default!;

    public static BankTransaction Create(TransactionStatusType status, int bankAccountId, int userId, decimal amount, string transactionNumber, string description)
        => new()
        {
            Status = status,
            Amount = amount,
            UserId = userId,
            Description = description,
            BankAccountId = bankAccountId,
            TransactionNumber = transactionNumber
        };

    public void MarkAsCompleted(string transactionNumber)
    {
        if (Status != TransactionStatusType.Pending)
            throw new InvalidOperationException("Only pending bank transactions can be completed.");

        Status = TransactionStatusType.Completed;
        TransactionNumber = transactionNumber;
        TransactionDateOnUtc = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        if (Status != TransactionStatusType.Pending)
            throw new InvalidOperationException("Only pending bank transactions can be marked as failed.");

        Status = TransactionStatusType.Failed;
    }

    public void SetBankAccount(BankAccount bankAccount)
    {
        BankAccount = bankAccount;
    }
}

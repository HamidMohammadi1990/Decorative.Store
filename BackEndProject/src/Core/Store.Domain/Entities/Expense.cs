using Store.Domain.Common;

namespace Store.Domain.Entities;

public class Expense : BaseEntity
{
    public int? UserId { get; set; }
    public int FinancialDocumentId { get; set; } = default!;
    public int? BankTransactionId { get; set; }
    public int? ChequeTransactionId { get; set; }
    public int? WalletTransactionId { get; set; }
    public decimal Amount { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime ExpenseDate { get; set; } = default!;
    public int ExpenseTypeId { get; set; } = default!;

    public ExpenseType ExpenseType { get; set; } = default!;
    public BankTransaction BankTransaction { get; set; } = default!;
    public ChequeTransaction ChequeTransaction { get; set; } = default!;
    public FinancialDocument FinancialDocument { get; set; } = default!;
    public WalletTransaction WalletTransaction { get; set; } = default!;
}

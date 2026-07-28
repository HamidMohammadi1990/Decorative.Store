using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class ChequeTransaction : BaseEntity
{
    public int? UserId { get; set; }
    public int? CompanyId { get; set; }
    public string CheckNumber { get; set; } = default!;
    public int BankId { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
    public DateTime IssueDate { get; set; } = default!;
    public DateTime DueDate { get; set; } = default!;
    public CheckStatusType CheckStatus { get; set; } = default!;
    public int FinancialDocumentId { get; set; } = default!;
    public string SayadTrackingNumber { get; set; } = default!;
    public string? FileName { get; set; }


    public Bank Bank { get; set; } = default!;
    public User User { get; set; } = default!;
    public Company Company { get; set; } = default!;
    public FinancialDocument FinancialDocument { get; set; } = default!;
    public ICollection<Expense> Expenses { get; set; } = default!;
}
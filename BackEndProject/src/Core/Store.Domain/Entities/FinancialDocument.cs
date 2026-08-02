using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class FinancialDocument : BaseEntity
{
    public int FinancialYearId { get; private set; } = default!;
    public int? OrderId { get; private set; }
    public DateTime DocumentDateOnUtc { get; private set; } = DateTime.UtcNow;
    public string DocumentNumber { get; private set; } = default!;
    public FinancialDocumentType Type { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public int? RelatedRefundDocumentId { get; private set; }


    public Order Order { get; set; } = default!;
    public FinancialYear FinancialYear { get; set; } = default!;
    public FinancialDocument RefundedFinancialDocument { get; set; } = default!;
    public ICollection<Expense> Expenses { get; set; } = default!;
    public ICollection<OrderVat> OrderVats { get; set; } = [];
    public ICollection<BankTransaction> BankTransactions { get; set; } = [];
    public ICollection<OrderCommission> OrderCommissions { get; set; } = default!;
    public ICollection<WalletTransaction> WalletTransactions { get; set; } = [];
    public ICollection<ChequeTransaction> ChequeTransactions { get; set; } = default!;
    public ICollection<FinancialDocument> RefundedFinancialDocuments { get; set; } = default!;
    public ICollection<FinancialDocumentDetail> FinancialDocumentDetails { get; set; } = [];


    public static FinancialDocument Create(int orderId, FinancialDocumentType type, string description, string documentNumber, int financialYearId)
        => new()
        {
            Type = type,
            OrderId = orderId,
            Description = description,
            DocumentNumber = documentNumber,
            FinancialYearId = financialYearId
        };

    public static FinancialDocument CreateForWallet(
        FinancialDocumentType type,
        string description,
        string documentNumber,
        int financialYearId)
        => new()
        {
            Type = type,
            Description = description,
            DocumentNumber = documentNumber,
            FinancialYearId = financialYearId
        };

    public void AddDetail(FinancialDocumentDetail financialDocumentDetail)
    {
        FinancialDocumentDetails.Add(financialDocumentDetail);
    }

    public void AddBankTransaction(BankTransaction bankTransaction)
    {
        BankTransactions.Add(bankTransaction);
    }

    public void AddWalletTransaction(WalletTransaction walletTransaction)
    {
        WalletTransactions.Add(walletTransaction);
    }
}
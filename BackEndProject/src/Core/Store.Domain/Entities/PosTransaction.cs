using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class PosTransaction : BaseEntity
{
    public int CompanyPosDeviceId { get; set; } = default!;
    public int FinancialDocumentId { get; set; } = default!;
    public PosTransactionStatusType Status { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
    public string TransactionNumber { get; set; } = default!;
    public string CardNumber { get; set; } = default!;
    public DateTime PaymentDate { get; set; } = default!;
    public string Description { get; set; } = default!;


    public FinancialDocument FinancialDocument { get; set; } = default!;
    public CompanyPosDevice CompanyPosDevice { get; set; } = default!;
}
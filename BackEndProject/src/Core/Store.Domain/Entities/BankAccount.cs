using Store.Domain.Common;

namespace Store.Domain.Entities;

public class BankAccount : BaseEntity
{
    public int BankId { get; set; }
    public string Title { get; set; } = default!;
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; }
    public string PaymentUrl { get; set; } = default!;
    public string VerifyPaymentUrl { get; set; } = default!;
    public string SuccessCallBackUrl { get; set; } = default!;
    public string FailureCallBackUrl { get; set; } = default!;
    public string MerchantCode { get; set; } = default!;
    public string? ApiKey { get; set; }
    public string? Dscription { get; set; }


    public Bank Bank { get; set; } = default!;
    public ICollection<BankTransaction> BankTransactions { get; private set; } = default!;
}
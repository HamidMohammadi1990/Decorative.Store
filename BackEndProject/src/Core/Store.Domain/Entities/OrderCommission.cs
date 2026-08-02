using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class OrderCommission : BaseEntity
{
    public int OrderId { get; set; } = default!;
    public int FinancialDocumentId { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
    public DateTime CreatedOnUtc { get; set; } = default!;
    public DateTime? RefundedOnUtc { get; set; }
    public OrderCommissionStatusType Status { get; set; } = default!;

    public Order Order { get; set; } = default!;
    public FinancialDocument FinancialDocument { get; set; } = default!;
}

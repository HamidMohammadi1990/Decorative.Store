using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class OrderVat : BaseEntity
{
    public int OrderId { get; private set; } = default!;
    public int FinancialDocumentId { get; private set; } = default!;
    public decimal Amount { get; private set; } = default!;
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? RefundedOnUtc { get; private set; }
    public OrderVatStatusType Status { get; private set; } = default!;


    public Order Order { get; set; } = default!;
    public FinancialDocument FinancialDocument { get; set; } = default!;

    public static OrderVat Create(
        int orderId,
        int financialDocumentId,
        decimal amount,
        OrderVatStatusType status)
        => new()
        {
            Amount = amount,
            Status = status,
            OrderId = orderId,
            FinancialDocumentId = financialDocumentId
        };

    public void MarkAsPaid()
    {
        if (Status == OrderVatStatusType.Paid)
            return;

        Status = OrderVatStatusType.Paid;
    }
}
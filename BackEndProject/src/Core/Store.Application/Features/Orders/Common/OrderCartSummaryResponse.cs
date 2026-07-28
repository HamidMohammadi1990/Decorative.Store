namespace Edition.Application.Features.Orders.Common;

public record OrderCartSummaryResponse
{
    public decimal TotalPrice { get; init; }
    public decimal DiscountAmount { get; init; }
    public decimal FinalPrice { get; init; }
    public bool IsDiscountApplied { get; init; }
    public bool IsDiscountInvalidated { get; init; }
    public string? DiscountInvalidationMessage { get; init; }
}
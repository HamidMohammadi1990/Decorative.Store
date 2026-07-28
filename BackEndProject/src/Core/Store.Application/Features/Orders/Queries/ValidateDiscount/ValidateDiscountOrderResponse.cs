namespace Edition.Application.Features.Orders.Queries;

public record ValidateDiscountOrderResponse
{
    public string DiscountCode { get; init; } = null!;
    public decimal TotalPrice { get; init; }
    public decimal DiscountAmount { get; init; }
    public decimal FinalPrice { get; init; }
}

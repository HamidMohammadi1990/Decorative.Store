namespace Store.Domain.Dtos.ProductPriceDeliveryOptions;

public record GetAllProductPriceDeliveryOptionResponseDto
{
    public int Id { get; init; }
    public decimal Price { get; init; }
    public int ProductId { get; init; }
    public string ProductTitle { get; init; } = default!;
    public decimal CooperationPrice { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public int CompanyId { get; init; }
    public string CompanyName { get; init; } = default!;
    public int ProductPriceId { get; init; }
    public int DeliveryOptionId { get; init; }
    public string DeliveryOptionTitle { get; init; } = default!;
}
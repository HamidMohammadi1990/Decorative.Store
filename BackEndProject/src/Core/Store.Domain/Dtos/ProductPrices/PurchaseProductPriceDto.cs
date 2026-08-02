using Store.Domain.Dtos.Others;

namespace Store.Domain.Dtos.ProductPrices;

public record PurchaseProductPriceDto
{
    public int Id { get; init; }
    public PriceField Price { get; init; } = default!;
    public PriceField CooperationPrice { get; init; } = default!;


    public static PurchaseProductPriceDto Create(PriceField price, PriceField CooperationPrice)
        => new()
        {
            Price = price,
            CooperationPrice = CooperationPrice
        };
}
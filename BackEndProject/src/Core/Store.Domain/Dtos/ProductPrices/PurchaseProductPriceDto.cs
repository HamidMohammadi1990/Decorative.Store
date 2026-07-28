using Store.Domain.Dtos.Others;

namespace Store.Domain.Dtos.ProductPrices;

public record PurchaseProductPriceDto
{
    public PriceField Price { get; private set; } = default!;
    public PriceField CooperationPrice { get; private set; } = default!;


    public static PurchaseProductPriceDto Create(PriceField price, PriceField CooperationPrice)
        => new()
        {
            Price = price,
            CooperationPrice = CooperationPrice
        };
}
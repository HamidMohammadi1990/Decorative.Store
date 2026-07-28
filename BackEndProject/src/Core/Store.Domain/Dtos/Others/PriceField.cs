namespace Store.Domain.Dtos.Others;

public class PriceField : IComparable<PriceField>
{
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; } = default!;


    public static PriceField Create(string currencyCode)
        => new()
        {
            CurrencyCode = currencyCode
        };

    public static PriceField Create(string currencyCode, decimal amount)
        => new()
        {
            Amount = amount,
            CurrencyCode = currencyCode
        };

    public static PriceField Create(decimal amount)
        => new()
        {
            Amount = amount,
            CurrencyCode = "IRR"
        };

    public int CompareTo(PriceField? price)
    {
        if (price is null) return 1;

        if (CurrencyCode != price.CurrencyCode)
            throw new InvalidOperationException("Cannot compare PriceFields with different currency codes.");

        return Amount.CompareTo(price.Amount);
    }
}
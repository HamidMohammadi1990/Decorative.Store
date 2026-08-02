namespace Store.Domain.Dtos.PropertyItemPrices;

public record GetAllPropertyItemPriceDto
{
    public int Id { get; init; }
    public int PropertyItemId { get; init; }
    public string PropertyItemTitle { get; init; } = default!;
    public int PropertyId { get; init; }
    public string PropertyTitle { get; init; } = default!;
    public int PropertyCategoryId { get; init; }
    public string PropertyCategoryTitle { get; init; } = default!;
    public decimal Price { get; init; }
    public decimal CooperationPrice { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public bool IsActive { get; init; }
}
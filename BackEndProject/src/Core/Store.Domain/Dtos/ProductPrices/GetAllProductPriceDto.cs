namespace Store.Domain.Dtos.ProductPrices;

public record GetAllProductPriceDto
{
    public int Id { get; init; }
    public int CompanyId { get; init; }
    public string CompanyName { get; init; } = default!;
    public int UserId { get; init; }
    public string UserFirstName { get; init; } = default!;
    public string UserLastName { get; init; } = default!;
    public int ProductId { get; init; }
    public string ProductTitle { get; init; } = default!;
    public decimal Price { get; init; }
    public decimal CooperationPrice { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public bool IsActive { get; init; }
}
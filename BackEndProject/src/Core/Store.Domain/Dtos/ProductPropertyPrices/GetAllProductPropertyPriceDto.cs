namespace Store.Domain.Dtos.ProductPropertyPrices;

public record GetAllProductPropertyPriceDto
{
    public int Id { get; init; }
    public int CompanyId { get; init; }
    public int ProductPropertyId { get; init; }
    public decimal Price { get; init; }
    public decimal CooperationPrice { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public bool IsActive { get; init; }
    public int UserId { get; init; }
    public int ProductId { get; init; }
    public string? UserFirstName { get; set; } = default!;
    public string? UserLastName { get; set; } = default!;
    public string ProductTitle { get; init; } = default!;
}
namespace Store.Domain.Dtos.ProductDescriptions;

public record GetAllProductDescriptionResponseDto
{
    public int Id { get; init; }
    public string Description { get; init; } = default!;
    public int ProductId { get; init; }
    public string ProductTitle { get; init; } = default!;
}
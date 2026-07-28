namespace Store.Domain.Dtos.ProductDescriptions;

public record SearchProductDescriptionResponseDto
{
    public int Id { get; init; }
    public string Description { get; init; } = default!;
    public int ProductId { get; init; }
}
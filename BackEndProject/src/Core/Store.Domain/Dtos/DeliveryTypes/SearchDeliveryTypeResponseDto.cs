namespace Store.Domain.Dtos.DeliveryTypes;

public record SearchDeliveryTypeResponseDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public int Priority { get; init; }
}
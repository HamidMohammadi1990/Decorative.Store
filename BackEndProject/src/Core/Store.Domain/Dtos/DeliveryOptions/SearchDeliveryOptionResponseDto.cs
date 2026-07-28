namespace Store.Domain.Dtos.DeliveryOptions;

public record SearchDeliveryOptionResponseDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public int DeliveryDays { get; init; }
}
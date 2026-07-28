namespace Store.Domain.Dtos.DeliveryOptions;

public record GetAllDeliveryOptionResponseDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public int DeliveryDays { get; init; }
    public bool IsActive { get; init; }
}
namespace Store.Domain.Dtos.DeliveryTypes;

public record GetAllDeliveryTypeResponseDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public bool IsActive { get; init; }
    public int Priority { get; init; }
}
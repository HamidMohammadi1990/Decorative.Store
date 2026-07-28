namespace Edition.Application.Features.DeliveryTypes.Queries;

public record SearchDeliveryTypeResponse
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public int Priority { get; init; }
}
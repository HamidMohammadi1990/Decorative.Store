namespace Edition.Application.Features.DeliveryTypes.Queries;

public record GetAllDeliveryTypeResponse
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public bool IsActive { get; init; }
    public int Priority { get; init; }
}
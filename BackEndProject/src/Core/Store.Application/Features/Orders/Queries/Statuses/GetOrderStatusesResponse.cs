namespace Edition.Application.Features.Orders.Queries;

public record GetOrderStatusResponse
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
}

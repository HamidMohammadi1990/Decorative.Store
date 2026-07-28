using Store.Common.Models;

namespace Edition.Application.Features.DeliveryOptions.Commands;

public record CreateDeliveryOptionRequest : IRequest<OperationResult<CreateDeliveryOptionResponse>>
{
    public string Title { get; init; } = default!;
    public int DeliveryDays { get; init; }
}
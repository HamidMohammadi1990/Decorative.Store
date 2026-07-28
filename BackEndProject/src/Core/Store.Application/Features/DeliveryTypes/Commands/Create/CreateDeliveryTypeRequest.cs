using Store.Common.Models;

namespace Edition.Application.Features.DeliveryTypes.Commands;

public record CreateDeliveryTypeRequest : IRequest<OperationResult<CreateDeliveryTypeResponse>>
{
    public string Title { get; init; } = default!;
    public int Priority { get; init; }
}
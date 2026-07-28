using Store.Common.Models;

namespace Edition.Application.Features.Orders.Queries;

public record ValidateDiscountOrderRequest : IRequest<OperationResult<ValidateDiscountOrderResponse>>
{
    public string DiscountCode { get; init; } = null!;
}

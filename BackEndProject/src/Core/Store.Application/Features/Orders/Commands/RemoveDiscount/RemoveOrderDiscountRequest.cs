using Store.Common.Models;

namespace Edition.Application.Features.Orders.Commands;

public record RemoveOrderDiscountRequest : IRequest<OperationResult<RemoveOrderDiscountResponse>>;

using Store.Common.Models;

namespace Edition.Application.Features.Orders.Queries;

public record GetStatusSummaryOrderRequest : IRequest<OperationResult<List<GetStatusSummaryOrderResponse>>> { }
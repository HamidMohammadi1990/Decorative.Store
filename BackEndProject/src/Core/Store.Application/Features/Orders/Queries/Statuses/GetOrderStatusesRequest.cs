using Store.Common.Models;

namespace Edition.Application.Features.Orders.Queries;

public record GetOrderStatusesRequest : IRequest<OperationResult<List<GetOrderStatusResponse>>> { }

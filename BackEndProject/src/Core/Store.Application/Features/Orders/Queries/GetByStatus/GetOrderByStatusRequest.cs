using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Queries;

public record GetOrderByStatusRequest : IRequest<OperationResult<List<GetOrderByStatusResponse>>>
{
    public OrderStatusType Status { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
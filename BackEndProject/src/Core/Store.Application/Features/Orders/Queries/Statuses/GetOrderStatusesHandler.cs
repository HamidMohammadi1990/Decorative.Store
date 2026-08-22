using Store.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Queries;

public class GetOrderStatusesHandler
    : IRequestHandler<GetOrderStatusesRequest, OperationResult<List<GetOrderStatusResponse>>>
{
    public Task<OperationResult<List<GetOrderStatusResponse>>> Handle(
        GetOrderStatusesRequest request,
        CancellationToken cancellationToken)
    {
        var statuses = Enum.GetValues<OrderStatusType>()
            .Select(status => new GetOrderStatusResponse
            {
                Id = (int)status,
                Title = status.ToDisplay(),
            })
            .ToList();

        return Task.FromResult<OperationResult<List<GetOrderStatusResponse>>>(statuses);
    }
}

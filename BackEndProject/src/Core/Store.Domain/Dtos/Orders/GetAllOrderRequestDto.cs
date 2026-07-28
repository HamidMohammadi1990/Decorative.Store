using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Orders;

public record GetAllOrderRequestDto : IContentPolicyQueryDto<Order>
{
    [QueryFilter(MemberPath = "order.UserId")]
    public int? UserId { get; init; }

    [QueryFilter(MemberPath = "order.IsFinaly")]
    public bool? IsFinaly { get; init; }

    [QueryFilter(MemberPath = "order.Status")]
    public OrderStatusType? Status { get; init; } = OrderStatusType.Completed;

    [QueryFilter(MemberPath = "order.TrackingCode")]
    public long? TrackingCode { get; init; }

    [QueryFilter(MemberPath = "order.Title", Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<Order, bool>>? ContentFilter { get; set; }
}

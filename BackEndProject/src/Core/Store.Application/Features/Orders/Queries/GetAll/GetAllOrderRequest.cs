using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Queries;

public record GetAllOrderRequest : ContentPolicyRequest<Order>, IRequest<OperationResult<PagedResult<GetAllOrderResponse>>>
{
    public long? TrackingCode { get; init; }
    public string? Title { get; init; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? UserId { get; init; }

    public OrderStatusType? Status { get; init; } = OrderStatusType.Completed;
    public bool? IsFinaly { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}
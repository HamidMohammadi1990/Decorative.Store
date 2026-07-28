using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.DeliveryTypes.Queries;

public record SearchDeliveryTypeRequest : ContentPolicyRequest<DeliveryType>, IRequest<OperationResult<PagedResult<SearchDeliveryTypeResponse>>>
{
    public string? Title { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.DeliveryOptions.Queries;

public record SearchDeliveryOptionRequest : ContentPolicyRequest<DeliveryOption>, IRequest<OperationResult<PagedResult<SearchDeliveryOptionResponse>>>
{
    public string? Title { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.DeliveryOptions.Queries;

public record GetAllDeliveryOptionRequest : ContentPolicyRequest<DeliveryOption>, IRequest<OperationResult<PagedResult<GetAllDeliveryOptionResponse>>>
{
    public string? Title { get; init; }
    public int? DeliveryDays { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
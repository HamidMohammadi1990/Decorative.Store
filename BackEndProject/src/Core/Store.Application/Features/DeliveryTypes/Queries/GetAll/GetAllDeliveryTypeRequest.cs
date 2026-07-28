using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.DeliveryTypes.Queries;

public record GetAllDeliveryTypeRequest : ContentPolicyRequest<DeliveryType>, IRequest<OperationResult<PagedResult<GetAllDeliveryTypeResponse>>>
{
    public string? Title { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
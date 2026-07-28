using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.Provinces.Queries;

public record GetAllProvinceRequest : ContentPolicyRequest<Province>, IRequest<OperationResult<PagedResult<GetAllProvinceResponse>>>
{
    public string? Name { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
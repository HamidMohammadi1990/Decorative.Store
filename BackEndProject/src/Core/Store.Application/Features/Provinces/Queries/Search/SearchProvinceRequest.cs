using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.Provinces.Queries;

public record SearchProvinceRequest : ContentPolicyRequest<Province>, IRequest<OperationResult<PagedResult<SearchProvinceResponse>>>
{
    public string? Name { get; init; }    
    public PagedRequest Pagination { get; init; } = default!;
}
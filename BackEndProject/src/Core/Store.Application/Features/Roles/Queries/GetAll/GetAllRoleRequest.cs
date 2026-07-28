using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.Roles.Queries;

public record GetAllRoleRequest : ContentPolicyRequest<Role>, IRequest<OperationResult<PagedResult<GetAllRoleResponse>>>
{
    public string? Title { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
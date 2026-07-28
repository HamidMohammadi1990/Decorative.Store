using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;
using Store.Domain.Entities;

namespace Edition.Application.Features.Permissions.Queries;

public record GetAllPermissionRequest : ContentPolicyRequest<Permission>, IRequest<OperationResult<PagedResult<GetAllPermissionResponse>>>
{
    public string? Title { get; init; } = null!;
    public string? Url { get; init; } = null!;
    public string? NameSpace { get; init; }
    public PermissionType? ParentId { get; init; }
    public PermissionLevelType? LevelTypeId { get; init; }
    public bool? IsActive { get; init; } = true;
    public PagedRequest Pagination { get; init; } = default!;
}
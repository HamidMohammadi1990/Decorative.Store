using System.Linq.Expressions;
using Edition.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Domain.Dtos.RolePermissions;

public record GetAllRolePermissionRequestDto : IContentPolicyQueryDto<RolePermission>
{
    [QueryFilter(MemberPath = "rolePermission.RoleId")]
    public int? RoleId { get; init; }

    [QueryFilter(MemberPath = "rolePermission.PermissionId")]
    public PermissionType? PermissionId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<RolePermission, bool>>? ContentFilter { get; set; }
}

public record GetAllRolePermissionDto
{
    public int Id { get; init; }
    public int RoleId { get; init; }
    public string RoleTitle { get; init; } = default!;
    public PermissionType PermissionId { get; init; }
    public string PermissionTitle { get; init; } = default!;
}

public record GetRolePermissionDto
{
    public int Id { get; init; }
    public int RoleId { get; init; }
    public string RoleTitle { get; init; } = default!;
    public PermissionType PermissionId { get; init; }
    public string PermissionTitle { get; init; } = default!;
}

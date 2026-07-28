using System.Linq.Expressions;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.UserRoles;

public record GetAllUserRoleRequestDto : IContentPolicyQueryDto<UserRole>
{
    [QueryFilter(MemberPath = "userRole.UserId")]
    public int? UserId { get; init; }

    [QueryFilter(MemberPath = "userRole.RoleId")]
    public int? RoleId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<UserRole, bool>>? ContentFilter { get; set; }
}

public record GetAllUserRoleDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string UserName { get; init; } = default!;
    public int RoleId { get; init; }
    public string RoleTitle { get; init; } = default!;
}

public record GetUserRoleDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string UserName { get; init; } = default!;
    public int RoleId { get; init; }
    public string RoleTitle { get; init; } = default!;
}

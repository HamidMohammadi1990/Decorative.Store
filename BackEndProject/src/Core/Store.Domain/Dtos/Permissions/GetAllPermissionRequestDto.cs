using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Permissions;

public record GetAllPermissionRequestDto : IContentPolicyQueryDto<Permission>
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Title { get; init; } = null!;

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Url { get; init; } = null!;

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? NameSpace { get; init; }

    [QueryFilter]
    public PermissionType? ParentId { get; init; }

    [QueryFilter]
    public PermissionLevelType? LevelTypeId { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; } = true;

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<Permission, bool>>? ContentFilter { get; set; }
}

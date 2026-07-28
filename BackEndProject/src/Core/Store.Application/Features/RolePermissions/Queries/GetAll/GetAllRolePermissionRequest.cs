using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;
using Store.Domain.Entities;

namespace Edition.Application.Features.RolePermissions.Queries;

public record GetAllRolePermissionRequest : ContentPolicyRequest<RolePermission>, IRequest<OperationResult<PagedResult<GetAllRolePermissionResponse>>>
{
    [JsonConverter(typeof(RoleNullableEncryptor))]
    public int? RoleId { get; init; }

    [JsonConverter(typeof(PermissionNullableEncryptor))]
    public PermissionType? PermissionId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}

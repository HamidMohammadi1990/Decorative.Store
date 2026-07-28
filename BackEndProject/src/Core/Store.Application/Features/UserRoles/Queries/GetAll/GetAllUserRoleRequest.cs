using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.UserRoles.Queries;

public record GetAllUserRoleRequest : ContentPolicyRequest<UserRole>, IRequest<OperationResult<PagedResult<GetAllUserRoleResponse>>>
{
    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? UserId { get; init; }

    [JsonConverter(typeof(RoleNullableEncryptor))]
    public int? RoleId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}

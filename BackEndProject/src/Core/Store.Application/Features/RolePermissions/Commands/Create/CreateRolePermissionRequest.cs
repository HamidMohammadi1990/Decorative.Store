using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.RolePermissions.Commands;

public record CreateRolePermissionRequest : IRequest<OperationResult<CreateRolePermissionResponse>>
{
    [JsonConverter(typeof(RoleEncryptor))]
    public int RoleId { get; init; }

    [JsonConverter(typeof(PermissionEncryptor))]
    public PermissionType PermissionId { get; init; }
}

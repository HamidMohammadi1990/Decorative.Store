using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.RolePermissions.Commands;

public record CreateRolePermissionResponse
{
    [JsonConverter(typeof(RolePermissionEncryptor))]
    public int Id { get; init; }
}

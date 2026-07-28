using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Enums;

namespace Edition.Application.Features.RolePermissions.Queries;

public record GetAllRolePermissionResponse
{
    [JsonConverter(typeof(RolePermissionEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(RoleEncryptor))]
    public int RoleId { get; init; }

    public string RoleTitle { get; init; } = default!;

    [JsonConverter(typeof(PermissionEncryptor))]
    public PermissionType PermissionId { get; init; }

    public string PermissionTitle { get; init; } = default!;
}

using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.RolePermissions.Commands;

public record DeleteRolePermissionRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(RolePermissionEncryptor))]
    public int Id { get; init; }
}

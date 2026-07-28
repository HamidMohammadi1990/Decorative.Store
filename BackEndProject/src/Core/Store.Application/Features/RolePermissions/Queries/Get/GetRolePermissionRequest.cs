using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.RolePermissions.Queries;

public record GetRolePermissionRequest : IRequest<OperationResult<GetRolePermissionResponse?>>
{
    [JsonConverter(typeof(RolePermissionEncryptor))]
    public int Id { get; init; }
}

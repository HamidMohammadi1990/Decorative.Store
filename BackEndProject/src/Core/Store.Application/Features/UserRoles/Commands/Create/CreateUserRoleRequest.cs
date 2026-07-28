using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.UserRoles.Commands;

public record CreateUserRoleRequest : IRequest<OperationResult<CreateUserRoleResponse>>
{
    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    [JsonConverter(typeof(RoleEncryptor))]
    public int RoleId { get; init; }
}
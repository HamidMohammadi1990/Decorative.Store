using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.UserRoles.Commands;

public record CreateUserRoleResponse
{
    [JsonConverter(typeof(UserRoleEncryptor))]
    public int Id { get; init; }
}

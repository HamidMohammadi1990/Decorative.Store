using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Roles.Commands;

public record CreateRoleResponse
{
    [JsonConverter(typeof(RoleEncryptor))]
    public int Id { get; init; }
}
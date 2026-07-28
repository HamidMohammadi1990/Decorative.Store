using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.UserRoles.Queries;

public record GetUserRoleResponse
{
    [JsonConverter(typeof(UserRoleEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    public string UserName { get; init; } = default!;

    [JsonConverter(typeof(RoleEncryptor))]
    public int RoleId { get; init; }

    public string RoleTitle { get; init; } = default!;
}

using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Roles.Queries;

public record GetAllRoleResponse
{
    [JsonConverter(typeof(RoleEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public bool IsActive { get; init; }
}
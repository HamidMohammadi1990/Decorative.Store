using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Enums;

namespace Edition.Application.Features.Permissions.Queries;

public record GetPermissionResponse
{
    [JsonConverter(typeof(PermissionEncryptor))]
    public PermissionType Id { get; init; }

    public string Title { get; init; } = null!;
    public string Url { get; init; } = null!;
    public string? NameSpace { get; init; }

    [JsonConverter(typeof(PermissionNullableEncryptor))]
    public PermissionType? ParentId { get; init; }

    public PermissionLevelType LevelTypeId { get; init; }
    public int Priority { get; init; }
    public bool IsActive { get; init; }
}
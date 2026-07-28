using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Permissions.Commands;

public record CreatePermissionRequest : IRequest<OperationResult<CreatePermissionResponse>>
{
    [JsonConverter(typeof(PermissionEncryptor))]
    public PermissionType Id { get; init; }

    public string Title { get; init; } = default!;
    public string Url { get; init; } = default!;
    public string NameSpace { get; init; } = default!;
    public PermissionLevelType LevelTypeId { get; init; }

    [JsonConverter(typeof(PermissionNullableEncryptor))]
    public PermissionType? ParentId { get; init; }

    public int Priority { get; init; }
}

public record CreatePermissionResponse
{
    [JsonConverter(typeof(PermissionEncryptor))]
    public PermissionType Id { get; init; }
}

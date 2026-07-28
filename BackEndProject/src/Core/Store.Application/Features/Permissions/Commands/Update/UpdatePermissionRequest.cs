using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Permissions.Commands;

public record UpdatePermissionRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(PermissionEncryptor))]
    public PermissionType Id { get; init; }

    public string Title { get; init; } = default!;
    public string Url { get; init; } = default!;
    public string NameSpace { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
}

using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Permissions.Commands;

public record DeletePermissionRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(PermissionEncryptor))]
    public PermissionType Id { get; init; }
}

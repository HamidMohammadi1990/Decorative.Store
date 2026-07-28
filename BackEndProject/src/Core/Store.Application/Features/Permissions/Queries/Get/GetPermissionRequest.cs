using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Permissions.Queries;

public record GetPermissionRequest : IRequest<OperationResult<GetPermissionResponse?>>
{
    [JsonConverter(typeof(PermissionEncryptor))]
    public PermissionType Id { get; init; }
}
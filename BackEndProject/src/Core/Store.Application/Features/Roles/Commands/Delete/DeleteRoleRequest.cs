using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Roles.Commands;

public record DeleteRoleRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(RoleEncryptor))]
    public int Id { get; init; }
}
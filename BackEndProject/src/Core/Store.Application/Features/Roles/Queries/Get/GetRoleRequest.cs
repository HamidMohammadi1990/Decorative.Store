using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Roles.Queries;

public record GetRoleRequest : IRequest<OperationResult<GetRoleResponse?>>
{
    [JsonConverter(typeof(RoleEncryptor))]
    public int Id { get; init; }
}
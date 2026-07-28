using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.UserRoles.Queries;

public record GetUserRoleRequest : IRequest<OperationResult<GetUserRoleResponse?>>
{
    [JsonConverter(typeof(UserRoleEncryptor))]
    public int Id { get; init; }
}

using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Users.Queries;

public record GetUserRequest : IRequest<OperationResult<GetUserResponse?>>
{
    [JsonConverter(typeof(UserEncryptor))]
    public int Id { get; init; }
}
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Users.Commands;

public record DeleteUserRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(UserEncryptor))]
    public int Id { get; init; }
}

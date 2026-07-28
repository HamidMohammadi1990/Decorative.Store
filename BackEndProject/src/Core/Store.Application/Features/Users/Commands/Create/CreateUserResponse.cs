using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Users.Commands;

public record CreateUserResponse
{
    [JsonConverter(typeof(UserEncryptor))]
    public int Id { get; init; }
}
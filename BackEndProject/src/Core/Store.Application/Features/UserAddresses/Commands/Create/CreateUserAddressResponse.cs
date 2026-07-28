using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.UserAddresses.Commands;

public record CreateUserAddressResponse
{
    [JsonConverter(typeof(UserAddressEncryptor))]
    public int Id { get; init; }
}
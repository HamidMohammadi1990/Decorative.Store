using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Users.Commands;

public record CreateUserRequest : IRequest<OperationResult<CreateUserResponse>>
{
    [JsonConverter(typeof(CityEncryptor))]
    public int CityId { get; init; }
    public string UserName { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string? Email { get; init; }
    public string PhoneNumber { get; init; } = default!;
    public string Password { get; init; } = default!;
    public GenderType Gender { get; init; }
}
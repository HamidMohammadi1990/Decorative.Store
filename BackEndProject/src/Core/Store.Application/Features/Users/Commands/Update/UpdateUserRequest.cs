using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Users.Commands;

public record UpdateUserRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(UserEncryptor))]
    public int Id { get; init; }

    public string UserName { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string? Email { get; init; }
    public string PhoneNumber { get; init; } = default!;
    public string? Password { get; init; }
    public GenderType Gender { get; init; }
    public bool IsActive { get; init; }
    public bool LoginPermission { get; init; }
    public string? ProfileImageFileName { get; init; }
}

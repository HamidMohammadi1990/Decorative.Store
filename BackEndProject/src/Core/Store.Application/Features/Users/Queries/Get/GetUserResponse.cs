using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Enums;

namespace Edition.Application.Features.Users.Queries;

public record GetUserResponse
{
    [JsonConverter(typeof(UserEncryptor))]
    public int Id { get; init; }
    public string UserName { get; init; } = default!;
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public GenderType? Gender { get; init; }
}
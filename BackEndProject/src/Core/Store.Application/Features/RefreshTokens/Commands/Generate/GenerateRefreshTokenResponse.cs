using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.RefreshTokens.Commands;

public record GenerateRefreshTokenResponse
{
    public string AccessToken { get; init; } = default!;
    public string RefreshToken { get; init; } = default!;
    public string TokenType { get; init; } = default!;
    public int ExpiresIn { get; init; }

    [JsonConverter(typeof(UserSessionEncryptor))]
    public Guid SessionId { get; init; }
}

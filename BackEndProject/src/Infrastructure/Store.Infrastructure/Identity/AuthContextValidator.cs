using System.Security.Claims;
using Edition.Application.Contracts;
using Edition.Application.Models.Constants;
using Microsoft.AspNetCore.Identity;

namespace Store.Infrastructure.Identity;

public sealed class AuthContextValidator(
    IUserAuthCache userAuthCache,
    IAuthValidationState authValidationState)
    : IAuthContextValidator
{
    private static readonly string SecurityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;

    public async Task<bool> ValidateAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        if (authValidationState.CachedResult is bool cached)
            return cached;

        var result = await ValidateInternalAsync(principal, cancellationToken);
        authValidationState.CachedResult = result;
        return result;
    }

    private async Task<bool> ValidateInternalAsync(ClaimsPrincipal principal, CancellationToken cancellationToken)
    {
        var userIdValue = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdValue, out var userId))
            return false;

        var tokenSecurityStamp = principal.FindFirstValue(SecurityStampClaimType);
        if (string.IsNullOrWhiteSpace(tokenSecurityStamp))
            return false;

        if (!await userAuthCache.ValidateSecurityStampAsync(userId, tokenSecurityStamp, cancellationToken))
            return false;

        var sessionIdValue = principal.FindFirstValue(AuthClaimTypes.SessionId);
        if (string.IsNullOrWhiteSpace(sessionIdValue) || !Guid.TryParse(sessionIdValue, out var sessionId))
            return true;

        var jwtId = principal.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti);
        if (string.IsNullOrWhiteSpace(jwtId))
            return false;

        return await userAuthCache.ValidateSessionAsync(sessionId, userId, jwtId, cancellationToken);
    }
}

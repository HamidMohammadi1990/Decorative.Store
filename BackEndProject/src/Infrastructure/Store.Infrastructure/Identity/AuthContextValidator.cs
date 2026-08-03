using System.IdentityModel.Tokens.Jwt;
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
        var userIdValue = ResolveUserId(principal);
        if (!int.TryParse(userIdValue, out var userId))
            return false;

        var tokenSecurityStamp = ResolveSecurityStamp(principal);
        if (string.IsNullOrWhiteSpace(tokenSecurityStamp))
            return false;

        if (!await userAuthCache.ValidateSecurityStampAsync(userId, tokenSecurityStamp, cancellationToken))
            return false;

        var sessionIdValue = principal.FindFirstValue(AuthClaimTypes.SessionId);
        if (string.IsNullOrWhiteSpace(sessionIdValue) || !Guid.TryParse(sessionIdValue, out var sessionId))
            return true;

        var jwtId = ResolveJwtId(principal);
        if (string.IsNullOrWhiteSpace(jwtId))
            return false;

        return await userAuthCache.ValidateSessionAsync(sessionId, userId, jwtId, cancellationToken);
    }

    private static string? ResolveUserId(ClaimsPrincipal principal)
        => principal.FindFirstValue(ClaimTypes.NameIdentifier)
           ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

    private static string? ResolveSecurityStamp(ClaimsPrincipal principal)
        => principal.FindFirstValue(SecurityStampClaimType)
           ?? principal.FindFirstValue("AspNet.Identity.SecurityStamp");

    private static string? ResolveJwtId(ClaimsPrincipal principal)
        => principal.FindFirstValue(JwtRegisteredClaimNames.Jti)
           ?? principal.FindFirstValue("jti");
}

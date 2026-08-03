using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Edition.Application.Models.Constants;

namespace Edition.Application.Common.Security;

public static class AuthClaimResolver
{
    private static readonly string SecurityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;

    public static string? GetUserId(ClaimsPrincipal principal)
    {
        foreach (var claim in principal.Claims)
        {
            if (IsUserIdClaimType(claim.Type))
                return claim.Value;
        }

        return null;
    }

    public static string? GetJwtId(ClaimsPrincipal principal)
        => principal.FindFirstValue(JwtRegisteredClaimNames.Jti)
           ?? principal.FindFirstValue("jti")
           ?? FindClaimValue(principal, "jti");

    public static string? GetSessionId(ClaimsPrincipal principal)
        => principal.FindFirstValue(AuthClaimTypes.SessionId)
           ?? principal.FindFirstValue("sid")
           ?? FindClaimValue(principal, "sid");

    public static string? GetSecurityStamp(ClaimsPrincipal principal)
        => principal.FindFirstValue(SecurityStampClaimType)
           ?? principal.FindFirstValue("AspNet.Identity.SecurityStamp")
           ?? FindClaimValue(principal, "securitystamp", "serialnumber");

    private static bool IsUserIdClaimType(string claimType)
        => claimType is JwtRegisteredClaimNames.Sub or "sub" or ClaimTypes.NameIdentifier
           || claimType.Contains("nameidentifier", StringComparison.OrdinalIgnoreCase)
           || claimType.EndsWith("/sub", StringComparison.OrdinalIgnoreCase);

    private static string? FindClaimValue(ClaimsPrincipal principal, params string[] markers)
    {
        foreach (var claim in principal.Claims)
        {
            foreach (var marker in markers)
            {
                if (claim.Type.Contains(marker, StringComparison.OrdinalIgnoreCase))
                    return claim.Value;
            }
        }

        return null;
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Edition.Application.Models.Constants;

namespace Edition.Application.Common.Security;

public static class AuthClaimResolver
{
    private static readonly string SecurityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;

    public static string? GetUserId(ClaimsPrincipal principal)
        => principal.FindFirstValue(ClaimTypes.NameIdentifier)
           ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
           ?? FindClaimValue(principal, "nameidentifier");

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

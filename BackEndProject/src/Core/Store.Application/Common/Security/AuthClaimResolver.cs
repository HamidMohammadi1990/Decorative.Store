using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Edition.Application.Common.Security;

public static class AuthClaimResolver
{
    private static readonly string SecurityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;

    public static string? GetUserId(ClaimsPrincipal principal)
        => principal.FindFirstValue(ClaimTypes.NameIdentifier)
           ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

    public static string? GetJwtId(ClaimsPrincipal principal)
        => principal.FindFirstValue(JwtRegisteredClaimNames.Jti)
           ?? principal.FindFirstValue("jti");

    public static string? GetSecurityStamp(ClaimsPrincipal principal)
        => principal.FindFirstValue(SecurityStampClaimType)
           ?? principal.FindFirstValue("AspNet.Identity.SecurityStamp");
}

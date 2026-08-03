using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;

namespace Store.Common.Extensions;

public static class IdentityExtensions
{
    private const string SubClaimType = "sub";

    public static string? FindFirstValue(this ClaimsIdentity identity, string claimType)
    {
        return identity?.FindFirst(claimType)?.Value;
    }

    public static bool IsInRole(this IIdentity identity, string roleName)
    {
        return identity?.FindFirstValue(roleName) != null;
    }

    public static string? FindFirstValue(this IIdentity identity, string claimType)
    {
        var claimsIdentity = identity as ClaimsIdentity;
        return claimsIdentity?.FindFirstValue(claimType);
    }

    public static string? GetUserId(this ClaimsPrincipal? principal)
    {
        if (principal is null)
            return null;

        foreach (var identity in principal.Identities.OfType<ClaimsIdentity>())
        {
            var userId = identity.GetUserId();
            if (!string.IsNullOrWhiteSpace(userId))
                return userId;
        }

        return null;
    }

    public static string? GetUserId(this IIdentity? identity)
    {
        if (identity is not ClaimsIdentity claimsIdentity)
            return null;

        var userId = claimsIdentity.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? claimsIdentity.FindFirstValue(SubClaimType);

        if (!string.IsNullOrWhiteSpace(userId))
            return userId;

        return claimsIdentity.Claims.FirstOrDefault(IsUserIdClaimType)?.Value;
    }

    public static T? GetUserId<T>(this ClaimsPrincipal? principal)
    {
        return ParseUserId<T>(principal?.GetUserId());
    }

    public static T? GetUserId<T>(this IIdentity? identity)
    {
        return ParseUserId<T>(identity?.GetUserId());
    }

    public static string? GetUserName(this IIdentity identity)
    {
        if (identity is not ClaimsIdentity claimsIdentity)
            return null;

        return claimsIdentity.FindFirstValue(ClaimTypes.Name)
            ?? claimsIdentity.FindFirstValue("unique_name")
            ?? claimsIdentity.FindFirstValue(ClaimTypes.GivenName);
    }

    private static T? ParseUserId<T>(string? userId)
    {
        if (userId is null || !userId.HasValue())
            return default;

        var targetType = typeof(T);

        if (targetType == typeof(Guid))
            return Guid.TryParse(userId, out var guid) ? (T)(object)guid : default;

        if (targetType == typeof(int))
            return int.TryParse(userId, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue)
                ? (T)(object)intValue
                : default;

        if (targetType == typeof(long))
            return long.TryParse(userId, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue)
                ? (T)(object)longValue
                : default;

        if (targetType == typeof(string))
            return (T)(object)userId;

        try
        {
            return (T)Convert.ChangeType(userId, targetType, CultureInfo.InvariantCulture);
        }
        catch (InvalidCastException)
        {
            return default;
        }
        catch (FormatException)
        {
            return default;
        }
    }

    private static bool IsUserIdClaimType(Claim claim)
        => claim.Type is SubClaimType or ClaimTypes.NameIdentifier
           || claim.Type.Contains("nameidentifier", StringComparison.OrdinalIgnoreCase)
           || claim.Type.EndsWith("/sub", StringComparison.OrdinalIgnoreCase);
}
using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;

namespace Store.Common.Extensions;

public static class IdentityExtensions
{
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

    public static string? GetUserId(this IIdentity identity)
    {
        return identity?.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public static T? GetUserId<T>(this IIdentity identity)
    {
        var userId = identity?.GetUserId();
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

    public static string? GetUserName(this IIdentity identity)
    {
        return identity?.FindFirstValue(ClaimTypes.Name);
    }
}
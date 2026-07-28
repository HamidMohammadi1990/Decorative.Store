namespace Store.Api.Extensions;

/// <summary>
/// Get jwt token from authorization header
/// </summary>
public static class HttpContextAccessorExtensions
{
    public static string GetJwtToken(this IHttpContextAccessor httpContextAccessor)
        => httpContextAccessor.HttpContext!.Request.Headers
           .Authorization.ToString()
           .Replace("Bearer ", string.Empty, StringComparison.CurrentCultureIgnoreCase);
}
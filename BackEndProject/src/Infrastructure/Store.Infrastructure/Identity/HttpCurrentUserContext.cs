using Edition.Application.Common.Utilities;
using Edition.Application.Contracts;
using Edition.Application.Models.Constants;
using Edition.Application.Models.Services;
using Microsoft.AspNetCore.Http;
using Store.Common.Extensions;

namespace Store.Infrastructure.Identity;

public sealed class HttpCurrentUserContext(IHttpContextAccessor httpContextAccessor) : ICurrentUserContext
{
    public int UserId =>
        httpContextAccessor.HttpContext?.User.Identity?.GetUserId<int>() ?? 0;

    public Guid? SessionId
    {
        get
        {
            var sessionIdValue = httpContextAccessor.HttpContext?.User?.FindFirst(AuthClaimTypes.SessionId)?.Value;
            return Guid.TryParse(sessionIdValue, out var sessionId) ? sessionId : null;
        }
    }

    public bool IsAuthenticated =>
        httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public bool IsCooperation =>
        httpContextAccessor.HttpContext?.User.Identity?.IsInRole("Cooperation") ?? false;

    public string? ClientIp => ResolveClientIp();

    public UserSessionContext GetSessionContext()
    {
        var userAgent = httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();
        return new UserSessionContext(
            ClientIp,
            Truncate(userAgent, 512),
            UserAgentDeviceParser.Parse(userAgent),
            UserAgentDeviceParser.ParseDeviceType(userAgent),
            UserAgentDeviceParser.ParseOperatingSystem(userAgent));
    }

    private string? ResolveClientIp()
    {
        var forwardedHeader =
            httpContextAccessor.HttpContext?
                .Request
                .Headers["X-Forwarded-For"]
                .FirstOrDefault();

        var userIp =
            !string.IsNullOrWhiteSpace(forwardedHeader)
                ? forwardedHeader.Split(',')[0].Trim()
                : httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        return Truncate(userIp, 45);
    }

    private static string? Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return value.Length <= maxLength ? value : value[..maxLength];
    }
}

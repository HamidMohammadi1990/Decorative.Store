using System.Text;
using System.Security.Cryptography;
using Edition.Application.Contracts;
using Edition.Application.Models.Services;
using Edition.Application.Common.Caching.Enums;
using Edition.Application.Common.Caching.Abstractions;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Services;

public sealed class UserAuthCache
    (
        IDistributedCache cache,
        SiteSettings siteSettings,
        IUserRepository userRepository,
        IUserSessionRepository userSessionRepository)
    : IUserAuthCache
{
    private const CacheInstanceType CacheInstance = CacheInstanceType.UserTokens;

    public Task SetSessionStateAsync(Guid sessionId, CachedSessionState state, DateTime expiresOnUtc, CancellationToken cancellationToken = default)
        => cache.SetAsync(GetSessionStateKey(sessionId), state, expiresOnUtc, CacheInstance, extend: true, token: cancellationToken);

    public async Task UpdateSessionJwtIdAsync(Guid sessionId, int userId, string jwtId, DateTime expiresOnUtc, CancellationToken cancellationToken = default)
    {
        var state = await cache.GetAsync<CachedSessionState>(GetSessionStateKey(sessionId), CacheInstance, cancellationToken);
        state = state is null || state.UserId != userId
            ? new CachedSessionState(userId, jwtId, expiresOnUtc)
            : state with { CurrentJwtId = jwtId, ExpiresOnUtc = expiresOnUtc };

        await SetSessionStateAsync(sessionId, state, expiresOnUtc, cancellationToken);
    }

    public Task RemoveSessionStateAsync(Guid sessionId, CancellationToken cancellationToken = default)
        => cache.RemoveAsync(GetSessionStateKey(sessionId), CacheInstance, cancellationToken);

    public async Task MarkSessionRevokedAsync(Guid sessionId, DateTime expiresOnUtc, CancellationToken cancellationToken = default)
    {
        await RemoveSessionStateAsync(sessionId, cancellationToken);

        var ttl = expiresOnUtc - DateTime.UtcNow;
        if (ttl <= TimeSpan.Zero)
            ttl = TimeSpan.FromMinutes(5);

        await cache.SetAsync(
            GetRevokedSessionKey(sessionId),
            new RevokedSessionMarker(),
            ttl,
            CacheInstance,
            token: cancellationToken);
    }

    public Task<bool> IsSessionRevokedAsync(Guid sessionId, CancellationToken cancellationToken = default)
        => cache.ExistsAsync(GetRevokedSessionKey(sessionId), CacheInstance, cancellationToken);

    public async Task<bool> ValidateSessionAsync(Guid sessionId, int userId, string jwtId, CancellationToken cancellationToken = default)
    {
        if (await IsSessionRevokedAsync(sessionId, cancellationToken))
            return false;

        var state = await cache.GetAsync<CachedSessionState>(GetSessionStateKey(sessionId), CacheInstance, cancellationToken);
        state ??= await HydrateSessionFromDbAsync(sessionId, userId, cancellationToken);

        if (state is null)
            return false;

        if (state.UserId != userId)
            return false;

        if (state.ExpiresOnUtc <= DateTime.UtcNow)
            return false;

        return string.Equals(state.CurrentJwtId, jwtId, StringComparison.Ordinal);
    }

    public Task SetSecurityStampAsync(int userId, string securityStamp, CancellationToken cancellationToken = default)
    {
        var ttl = GetSecurityStampCacheTtl();
        return cache.SetAsync(GetSecurityStampKey(userId), securityStamp, ttl, CacheInstance, extend: true, token: cancellationToken);
    }

    public async Task<bool> ValidateSecurityStampAsync(int userId, string tokenSecurityStamp, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tokenSecurityStamp))
            return false;

        var cachedStamp = await cache.GetAsync<string>(GetSecurityStampKey(userId), CacheInstance, cancellationToken);
        if (!string.IsNullOrWhiteSpace(cachedStamp) && FixedTimeEquals(tokenSecurityStamp, cachedStamp))
            return true;

        var dbStamp = await HydrateSecurityStampFromDbAsync(userId, cancellationToken);
        if (string.IsNullOrWhiteSpace(dbStamp))
            return false;

        return FixedTimeEquals(tokenSecurityStamp, dbStamp);
    }

    private async Task<CachedSessionState?> HydrateSessionFromDbAsync(Guid sessionId, int userId, CancellationToken cancellationToken)
    {
        var session = await userSessionRepository.GetActiveAsync(sessionId, userId);
        if (session is null)
            return null;

        var state = new CachedSessionState(session.UserId, session.CurrentJwtId, session.ExpiresOnUtc);
        await SetSessionStateAsync(sessionId, state, session.ExpiresOnUtc, cancellationToken);
        return state;
    }

    private async Task<string?> HydrateSecurityStampFromDbAsync(int userId, CancellationToken cancellationToken)
    {
        var stamp = await userRepository.GetSecurityStampAsync(userId, cancellationToken);
        if (string.IsNullOrWhiteSpace(stamp))
            return null;

        await SetSecurityStampAsync(userId, stamp, cancellationToken);
        return stamp;
    }

    private TimeSpan GetSecurityStampCacheTtl()
    {
        var minutes = siteSettings.JwtSettings.RefreshTokenExpirationMinutes;
        return minutes > 0 ? TimeSpan.FromMinutes(minutes) : TimeSpan.FromDays(7);
    }

    private static bool FixedTimeEquals(string left, string right)
    {
        var leftBytes = Encoding.UTF8.GetBytes(left);
        var rightBytes = Encoding.UTF8.GetBytes(right);
        return leftBytes.Length == rightBytes.Length
               && CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }

    private static string GetSessionStateKey(Guid sessionId)
        => $"SessionState|{sessionId}";

    private static string GetRevokedSessionKey(Guid sessionId)
        => $"SessionRevoked|{sessionId}";

    private static string GetSecurityStampKey(int userId)
        => $"UserSecurityStamp|{userId}";
}
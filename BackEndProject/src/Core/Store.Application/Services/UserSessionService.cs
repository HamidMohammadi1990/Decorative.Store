using Edition.Application.Contracts;
using Edition.Application.Contracts;
using Edition.Application.Models.Services;
using Edition.Application.Common.Caching.Enums;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Edition.Application.Services;

public class UserSessionService
    (
        IUnitOfWork uow,
        IUserAuthCache userAuthCache,
        IUserSessionRepository userSessionRepository,
        IRefreshTokenRepository refreshTokenRepository,
        SiteSettings siteSettings)
    : IUserSessionService
{
    public async Task<UserSession> CreateSessionAsync(
        Guid sessionId,
        int userId,
        string jwtId,
        UserSessionContext context,
        DateTime expiresOnUtc,
        CancellationToken cancellationToken = default)
    {
        var session = UserSession.Create(
            sessionId,
            userId,
            jwtId,
            expiresOnUtc,
            context.IpAddress,
            context.UserAgent,
            context.DeviceName,
            context.DeviceType,
            context.OperatingSystem);

        userSessionRepository.Add(session);
        await uow.SaveChangesAsync(cancellationToken);
        await EnforceSessionLimitAsync(userId, cancellationToken);

        await userAuthCache.SetSessionStateAsync(
            sessionId,
            new CachedSessionState(userId, jwtId, expiresOnUtc),
            expiresOnUtc,
            cancellationToken);

        return session;
    }

    public async Task<UserSession?> ContinueSessionAsync(Guid sessionId, int userId, string jwtId, CancellationToken cancellationToken = default)
    {
        var session = await userSessionRepository.GetActiveAsync(sessionId, userId);
        if (session is null)
            return null;

        session.UpdateActivity(jwtId);
        await uow.SaveChangesAsync(cancellationToken);

        await userAuthCache.UpdateSessionJwtIdAsync(sessionId, userId, jwtId, session.ExpiresOnUtc, cancellationToken);

        return session;
    }

    public Task<bool> ValidateSessionAsync(Guid sessionId, int userId, string jwtId, CancellationToken cancellationToken = default)
        => userAuthCache.ValidateSessionAsync(sessionId, userId, jwtId, cancellationToken);

    public async Task RevokeSessionAsync(Guid sessionId, int userId, UserSessionRevokeReason reason, CancellationToken cancellationToken = default)
    {
        var session = await userSessionRepository.FindAsync(sessionId);
        if (session is null || session.UserId != userId || session.IsRevoked)
            return;

        session.Revoke(reason);
        await refreshTokenRepository.InvalidateBySessionAsync(sessionId);
        await uow.SaveChangesAsync(cancellationToken);
        await userAuthCache.MarkSessionRevokedAsync(sessionId, session.ExpiresOnUtc, cancellationToken);
    }

    public async Task RevokeAllSessionsAsync(int userId, UserSessionRevokeReason reason, Guid? exceptSessionId = null, CancellationToken cancellationToken = default)
    {
        var activeSessions = await userSessionRepository.GetActiveSessionsAsync(userId);
        await userSessionRepository.RevokeAllAsync(userId, reason, exceptSessionId);
        var sessionsToInvalidate = activeSessions
            .Where(x => exceptSessionId is null || x.Id != exceptSessionId)
            .ToList();

        foreach (var session in sessionsToInvalidate)
            await refreshTokenRepository.InvalidateBySessionAsync(session.Id);

        await uow.SaveChangesAsync(cancellationToken);

        foreach (var session in activeSessions.Where(x => exceptSessionId is null || x.Id != exceptSessionId))
            await userAuthCache.MarkSessionRevokedAsync(session.Id, session.ExpiresOnUtc, cancellationToken);
    }

    public async Task EnforceSessionLimitAsync(int userId, CancellationToken cancellationToken = default)
    {
        var maxSessions = siteSettings.JwtSettings.MaxConcurrentUserSessions;
        if (maxSessions <= 0)
            return;

        var activeCount = await userSessionRepository.CountActiveSessionsAsync(userId);
        if (activeCount <= maxSessions)
            return;

        var revokeCount = activeCount - maxSessions;
        var oldestSessions = await userSessionRepository.GetOldestActiveSessionsAsync(userId, revokeCount);
        foreach (var session in oldestSessions)
        {
            session.Revoke(UserSessionRevokeReason.SessionLimitExceeded);
            await refreshTokenRepository.InvalidateBySessionAsync(session.Id);
            await userAuthCache.MarkSessionRevokedAsync(session.Id, session.ExpiresOnUtc, cancellationToken);
        }

        await uow.SaveChangesAsync(cancellationToken);
    }

    public Task<List<UserSession>> GetActiveSessionsAsync(int userId, CancellationToken cancellationToken = default)
        => userSessionRepository.GetActiveSessionsAsync(userId);
}
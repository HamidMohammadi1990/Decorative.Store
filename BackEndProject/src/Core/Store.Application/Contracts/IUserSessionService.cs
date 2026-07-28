using Store.Domain.Repositories;
using Edition.Application.Models.Services;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Common.Caching.Enums;
using Edition.Application.Common.Caching.Abstractions;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Edition.Application.Contracts;

public interface IUserSessionService
{
    Task<UserSession> CreateSessionAsync(Guid sessionId, int userId, string jwtId, UserSessionContext context, DateTime expiresOnUtc, CancellationToken cancellationToken = default);
    Task<UserSession?> ContinueSessionAsync(Guid sessionId, int userId, string jwtId, CancellationToken cancellationToken = default);
    Task<bool> ValidateSessionAsync(Guid sessionId, int userId, string jwtId, CancellationToken cancellationToken = default);
    Task RevokeSessionAsync(Guid sessionId, int userId, UserSessionRevokeReason reason, CancellationToken cancellationToken = default);
    Task RevokeAllSessionsAsync(int userId, UserSessionRevokeReason reason, Guid? exceptSessionId = null, CancellationToken cancellationToken = default);
    Task EnforceSessionLimitAsync(int userId, CancellationToken cancellationToken = default);
    Task<List<UserSession>> GetActiveSessionsAsync(int userId, CancellationToken cancellationToken = default);
}

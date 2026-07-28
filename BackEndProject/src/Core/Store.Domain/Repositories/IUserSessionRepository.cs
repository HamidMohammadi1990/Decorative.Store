using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Domain.Repositories;

public interface IUserSessionRepository
{
    void Add(UserSession session);
    ValueTask<UserSession?> FindAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserSession?> GetActiveAsync(Guid id, int userId);
    Task<List<UserSession>> GetActiveSessionsAsync(int userId);
    Task<int> CountActiveSessionsAsync(int userId);
    Task<List<UserSession>> GetOldestActiveSessionsAsync(int userId, int take);
    Task RevokeAllAsync(int userId, UserSessionRevokeReason reason, Guid? exceptSessionId = null);
}
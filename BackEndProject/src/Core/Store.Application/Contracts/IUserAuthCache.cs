using Edition.Application.Models.Services;

namespace Edition.Application.Contracts;

public interface IUserAuthCache
{
    Task SetSessionStateAsync(Guid sessionId, CachedSessionState state, DateTime expiresOnUtc, CancellationToken cancellationToken = default);
    Task UpdateSessionJwtIdAsync(Guid sessionId, int userId, string jwtId, DateTime expiresOnUtc, CancellationToken cancellationToken = default);
    Task RemoveSessionStateAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task MarkSessionRevokedAsync(Guid sessionId, DateTime expiresOnUtc, CancellationToken cancellationToken = default);
    Task<bool> IsSessionRevokedAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task<bool> ValidateSessionAsync(Guid sessionId, int userId, string jwtId, CancellationToken cancellationToken = default);

    Task SetSecurityStampAsync(int userId, string securityStamp, CancellationToken cancellationToken = default);
    Task<bool> ValidateSecurityStampAsync(int userId, string tokenSecurityStamp, CancellationToken cancellationToken = default);
}

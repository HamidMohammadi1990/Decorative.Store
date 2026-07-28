namespace Edition.Application.Models.Services;

public sealed record CachedSessionState(int UserId, string CurrentJwtId, DateTime ExpiresOnUtc);

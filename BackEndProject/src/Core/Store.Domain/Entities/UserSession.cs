using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class UserSession : BaseEntity<Guid>
{
    public int UserId { get; private set; }
    public string CurrentJwtId { get; private set; } = default!;
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public string? DeviceName { get; private set; }
    public DeviceType DeviceType { get; private set; }
    public OperatingSystemType OperatingSystem { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime LastSeenOnUtc { get; private set; }
    public DateTime ExpiresOnUtc { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime? RevokedOnUtc { get; private set; }
    public UserSessionRevokeReason? RevokedReason { get; private set; }


    public User User { get; private set; } = default!;
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = default!;


    public static UserSession Create(
        Guid id,
        int userId,
        string jwtId,
        DateTime expiresOnUtc,
        string? ipAddress,
        string? userAgent,
        string? deviceName,
        DeviceType deviceType,
        OperatingSystemType operatingSystem)
    {
        var now = DateTime.UtcNow;
        return new UserSession
        {
            Id = id,
            UserId = userId,
            CurrentJwtId = jwtId,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            DeviceName = deviceName,
            DeviceType = deviceType,
            OperatingSystem = operatingSystem,
            CreatedOnUtc = now,
            LastSeenOnUtc = now,
            ExpiresOnUtc = expiresOnUtc
        };
    }

    public bool IsActive() => !IsRevoked && ExpiresOnUtc > DateTime.UtcNow;

    public void UpdateActivity(string jwtId)
    {
        CurrentJwtId = jwtId;
        LastSeenOnUtc = DateTime.UtcNow;
    }

    public void Revoke(UserSessionRevokeReason reason)
    {
        if (IsRevoked)
            return;

        IsRevoked = true;
        RevokedOnUtc = DateTime.UtcNow;
        RevokedReason = reason;
    }
}
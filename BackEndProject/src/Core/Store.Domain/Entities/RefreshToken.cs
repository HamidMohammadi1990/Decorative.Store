using Store.Domain.Common;

namespace Store.Domain.Entities;

public class RefreshToken : BaseEntity<long>
{
    public string JwtId { get; private set; } = default!;
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public DateTime ExpiredDateOnUtc { get; private set; }
    public bool Used { get; private set; }
    public bool Invalidated { get; private set; }
    public int UserId { get; private set; }
    public Guid? UserSessionId { get; private set; }


    public User User { get; private set; } = default!;
    public UserSession? UserSession { get; private set; }


    public static RefreshToken Create(int userId, string jwtId, DateTime expiredDate, Guid? userSessionId = null)
        => new()
        {
            UserId = userId,
            JwtId = jwtId,
            ExpiredDateOnUtc = expiredDate,
            UserSessionId = userSessionId
        };

    public void Invalidate()
    {
        Invalidated = true;
    }

    public void UsedToken()
    {
        Used = true;
    }
}
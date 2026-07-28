namespace Store.Common.Models;

public record SiteSettings
{
    public JwtSettings JwtSettings { get; set; } = null!;
}
public record JwtSettings
{
    public string SecretKey { get; set; } = null!;
    public string EncryptKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int NotBeforeMinutes { get; set; }
    public int ExpirationMinutes { get; set; }
    public int RefreshTokenExpirationMinutes { get; set; } = 10080;
    public int MaxConcurrentUserSessions { get; set; } = 5;
}
using System.IdentityModel.Tokens.Jwt;

namespace Edition.Application.Models.Services;

public record AccessTokenResponse(JwtSecurityToken SecurityToken, string RefreshToken, Guid SessionId)
{
    public string AccessToken { get; set; } = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
    public string RefreshToken { get; set; } = RefreshToken;
    public Guid SessionId { get; set; } = SessionId;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; } = (int)(SecurityToken.ValidTo - DateTime.UtcNow).TotalSeconds;
}

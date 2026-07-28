using Microsoft.IdentityModel.Tokens;

namespace Edition.Application.Common.Extensions;

public static class TokenValidationParametersExtensions
{
    public static TokenValidationParameters CloneWithLifetimeValidation(
        this TokenValidationParameters source,
        bool validateLifetime)
    {
        return new TokenValidationParameters
        {
            ClockSkew = source.ClockSkew,
            RequireSignedTokens = source.RequireSignedTokens,
            ValidateIssuerSigningKey = source.ValidateIssuerSigningKey,
            IssuerSigningKey = source.IssuerSigningKey,
            RequireExpirationTime = source.RequireExpirationTime,
            ValidateLifetime = validateLifetime,
            ValidateAudience = source.ValidateAudience,
            ValidAudience = source.ValidAudience,
            ValidateIssuer = source.ValidateIssuer,
            ValidIssuer = source.ValidIssuer,
            TokenDecryptionKey = source.TokenDecryptionKey,
            ValidAlgorithms = source.ValidAlgorithms,
            ValidTypes = source.ValidTypes
        };
    }
}

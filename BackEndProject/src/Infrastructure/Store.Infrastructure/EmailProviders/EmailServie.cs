using Edition.Application.Configurations.Email;
using Edition.Application.Common.Caching.Enums;
using Edition.Application.Contracts.Infrastructure;
using Edition.Application.Common.Caching.Abstractions;
using Store.Common.Utilities;

namespace Store.Infrastructure.EmailProviders;

public class EmailServie
    (IDistributedCache cache, IEmailTokenProviderConfiguration emailTokenProviderConfiguration)
    : IEmailService
{
    public bool Send(string to, string message, string title)
    {
        return true;
    }

    public async Task<string> GenerateTokenAsync(string email)
    {
        var key = GetTokenKey(email);
        if (await cache.ExistsAsync(key, CacheInstanceType.Default))
            await cache.RemoveAsync(key, CacheInstanceType.Default);
        
        var token = NumberGenerator.Create(emailTokenProviderConfiguration.MinNumber,
            emailTokenProviderConfiguration.MaxNumber)
            .ToString($"D{emailTokenProviderConfiguration.DigitCount}");
        await cache.SetAsync(key, token, DateTime.UtcNow.AddSeconds(emailTokenProviderConfiguration.Duration), CacheInstanceType.Default, true);
        return token;
    }

    public async Task<bool> VerifyTokenAsync(string token, string email)
    {
        var key = GetTokenKey(email);
        var cachedToken = await cache.GetAsync<string>(key, CacheInstanceType.Default);
        if (cachedToken == null) return false;
        if (cachedToken != token) return false;
        await cache.RemoveAsync(key, CacheInstanceType.Default);
        return true;
    }

    private static string GetTokenKey(string email)
       => $"EmailConfirmation_{email}";
}
using Edition.Application.Configurations.SMS;
using Edition.Application.Common.Caching.Enums;
using Edition.Application.Common.Caching.Abstractions;
using Edition.Application.Contracts.Infrastructure;
using Store.Common.Utilities;

namespace Store.Infrastructure.SmsProviders;

public class SmsService
    (IDistributedCache cache, IPhoneNumberTokenProviderConfiguration phoneNumberTokenProviderConfiguration)
    : ISmsService
{
    public bool Send(string phoneNumber, string message)
    {
        return true;
    }

    public async Task<string> GenerateTokenAsync(string phoneNumber)
    {
        var instance = CacheInstanceType.Default;
        var cacheKey = GetTokenKey(phoneNumber);
        if (await cache.ExistsAsync(cacheKey, CacheInstanceType.Default))
            await cache.RemoveAsync(cacheKey, CacheInstanceType.Default);        
        var token =
            NumberGenerator.Create(
                phoneNumberTokenProviderConfiguration.MinNumber,
                phoneNumberTokenProviderConfiguration.MaxNumber)
            .ToString($"D{phoneNumberTokenProviderConfiguration.DigitCount}");

        cache.Set(cacheKey, token, new TimeSpan(0, 0, 0, phoneNumberTokenProviderConfiguration.Duration), instance);
        return token;
    }

    public async Task<bool> VerifyTokenAsync(string token, string phoneNumber)
    {
        var instance = CacheInstanceType.Default;
        var cacheKey = GetTokenKey(phoneNumber);
        var cachedToken = await cache.GetAsync<string>(cacheKey, instance);
        if (cachedToken == null) return false;
        if (cachedToken.ToString() != token) return false;
        await cache.RemoveAsync(cacheKey, instance);
        return true;
    }

    private static string GetTokenKey(string phoneNumber)
        => $"VerifyToken_{phoneNumber}";
}
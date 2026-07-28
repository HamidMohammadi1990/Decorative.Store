using RedLockNet.SERedis;
using StackExchange.Redis;
using RedLockNet.SERedis.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Edition.Application.Contracts.Infrastructure;
using Edition.Application.Common.Caching.Abstractions;
using Edition.Application.Contracts;
using Edition.Application.Contracts.Localization;
using Store.Infrastructure.Identity;
using Store.Infrastructure.Configurations;
using Store.Infrastructure.Localization;
using Store.Infrastructure.EmailProviders;
using Store.Infrastructure.Services;
using Store.Infrastructure.SmsProviders;
using Store.Infrastructure.CacheProviders;
using Store.Infrastructure.CacheProviders.Redis;

namespace Store.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        var redisConfiguration = configuration.GetSection("RedisConfiguration").Get<RedisConfiguration>();
        var redisHost = (redisConfiguration?.Hosts.FirstOrDefault())
                ?? throw new NullReferenceException($"{nameof(RedisConfiguration.Hosts)} is null in redis configuration!");

        var multiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions
        {
            Ssl = redisConfiguration.Ssl,
            Password = redisConfiguration.Password,
            AllowAdmin = redisConfiguration.AllowAdmin,
            DefaultDatabase = redisConfiguration.Database,
            ConnectRetry = redisConfiguration.ConnectRetry,
            ConnectTimeout = redisConfiguration.ConnectTimeout,
            EndPoints = { $"{redisHost.Host}:{redisHost.Port}" }
        });

        services.AddSingleton<IImageService, ImageService>();

        services.AddScoped<ICurrentUserContext, HttpCurrentUserContext>();
        services.AddScoped<CurrentLanguageContext>();
        services.AddScoped<ICurrentLanguageContext>(sp => sp.GetRequiredService<CurrentLanguageContext>());
        services.AddScoped<ICurrentLanguageContextInitializer>(sp => sp.GetRequiredService<CurrentLanguageContext>());
        services.AddScoped<IAuthValidationState, AuthValidationState>();
        services.AddScoped<IAuthContextValidator, AuthContextValidator>();

        services.AddScoped<ISmsService, SmsService>();
        services.AddScoped<IEmailService, EmailServie>();

        services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        services.AddSingleton<IDatabaseSelector, DatabaseSelector>();
        services.AddScoped<IRedisCacheService, RedisCacheService>();
        services.AddScoped<IDistributedCache, DistributedCache>();

        services.AddSingleton(sp =>
        {
            var redLockMultiplexer = new RedLockMultiplexer(multiplexer);
            return RedLockFactory.Create([redLockMultiplexer]);
        });

        return services;
    }
}
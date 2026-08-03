using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Edition.Application.Common.Utilities.JsonAttributes;
using Microsoft.AspNetCore.Identity;
using Edition.Application.Configurations.SMS;
using Edition.Application.Configurations.Email;
using Edition.Application.Configurations.ContentPolicies;
using Edition.Application.Configurations.Localization;
using Store.Api.Filters;
using Store.WebFramework.Api;

namespace Store.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddMinimalMvc(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Conventions.Add(new ControllerNameRouteConvention());
            options.Filters.Add<PermissionAuthorizeAttribute>();
            options.Filters.Add<LocalizationResultFilter>();
        }).AddNewtonsoftJson(option =>
        {
            option.SerializerSettings.ContractResolver = new EncryptorContractResolver();
            option.SerializerSettings.Converters.Add(new StringEnumConverter());
            option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
        services.AddSwaggerGenNewtonsoftSupport();
    }

    public static void AddEmailTokenProviderConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var emailConfig =
             configuration
             .GetSection(nameof(EmailTokenProviderConfiguration))
             .Get<EmailTokenProviderConfiguration>()
             ?? throw new NullReferenceException("EmailTokenProviderConfiguration is null ...");

        services.AddSingleton<IEmailTokenProviderConfiguration>(emailConfig!);
    }

    public static void AddPhoneNumberTokenProviderConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var phoneNumberConfig =
             configuration
             .GetSection(nameof(PhoneNumberTokenProviderConfiguration))
             .Get<PhoneNumberTokenProviderConfiguration>()
             ?? throw new NullReferenceException("PhoneNumberTokenProviderConfiguration is null ..."); ;

        services.AddSingleton<IPhoneNumberTokenProviderConfiguration>(phoneNumberConfig!);
    }

    public static void AddContentPolicyCacheConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var contentPolicyCacheConfig =
            configuration
                .GetSection(nameof(ContentPolicyCacheConfiguration))
                .Get<ContentPolicyCacheConfiguration>()
            ?? throw new NullReferenceException("ContentPolicyCacheConfiguration is null ...");

        services.AddSingleton<IContentPolicyCacheConfiguration>(contentPolicyCacheConfig);
    }

    public static void AddLanguageCacheConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var languageCacheConfig =
            configuration
                .GetSection(nameof(LanguageCacheConfiguration))
                .Get<LanguageCacheConfiguration>()
            ?? new LanguageCacheConfiguration();

        services.AddSingleton<ILanguageCacheConfiguration>(languageCacheConfig);
    }
}
using System.Globalization;
using Edition.Application.Configurations.Localization;
using Edition.Application.Contracts.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Localization;
using Store.Common.Localization;

namespace Store.Api.Extensions;

public static class LocalizationExtensions
{
    public static IServiceCollection AddEditionLocalization(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(nameof(LocalizationSettings))
            .Get<LocalizationSettings>() ?? new LocalizationSettings();

        services.AddSingleton(settings);
        services.AddSingleton<IResourceManager, EditionResourceManager>();

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.RequestCultureProviders =
            [
                new AcceptLanguageHeaderRequestCultureProvider(),
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider()
            ];

            options.ApplyCurrentCultureToResponseHeaders = true;
        });

        return services;
    }

    public static async Task ConfigureEditionLocalizationFromRegistryAsync(this WebApplication app)
    {
        var registry = app.Services.GetRequiredService<ILanguageRegistry>();
        var options = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;

        var activeLanguages = await registry.GetActiveLanguagesAsync();
        var defaultLanguage = await registry.GetDefaultAsync();

        var supportedCultures = activeLanguages
            .Select(x => new CultureInfo(x.Code))
            .ToList();

        if (supportedCultures.Count == 0)
            supportedCultures.Add(new CultureInfo(defaultLanguage.Code));

        options.DefaultRequestCulture = new RequestCulture(defaultLanguage.Code, defaultLanguage.Code);
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;

        options.RequestCultureProviders.Insert(0, new EditionAcceptLanguageCultureProvider(options));
    }

    public static IApplicationBuilder UseEditionLocalization(this IApplicationBuilder app)
        => app.UseRequestLocalization(
            app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
}

internal sealed class EditionAcceptLanguageCultureProvider(RequestLocalizationOptions options) : IRequestCultureProvider
{
    public Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var acceptLanguage = httpContext.Request.Headers.AcceptLanguage.ToString();
        if (string.IsNullOrWhiteSpace(acceptLanguage))
            return Task.FromResult<ProviderCultureResult?>(null);

        foreach (var segment in acceptLanguage.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var culture = LanguageCultureNormalizer.MapAcceptLanguageSegment(segment);
            if (culture is null)
                continue;

            if (IsSupported(culture))
                return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(culture, culture));
        }

        return Task.FromResult<ProviderCultureResult?>(null);
    }

    private bool IsSupported(string culture)
        => options.SupportedCultures.Any(x => string.Equals(x.Name, culture, StringComparison.OrdinalIgnoreCase))
           || options.SupportedUICultures.Any(x => string.Equals(x.Name, culture, StringComparison.OrdinalIgnoreCase));
}

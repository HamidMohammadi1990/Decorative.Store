using Edition.Application.Contracts.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Store.Common.Localization;

namespace Store.Api.Middlewares;

public sealed class CurrentLanguageMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        ILanguageRegistry languageRegistry,
        ICurrentLanguageContextInitializer languageContextInitializer)
    {
        var language = await ResolveLanguageAsync(context, languageRegistry);
        languageContextInitializer.Initialize(language.Id, language.Code);
        await next(context);
    }

    private static async Task<LanguageInfo> ResolveLanguageAsync(
        HttpContext context,
        ILanguageRegistry languageRegistry)
    {
        var acceptLanguage = context.Request.Headers.AcceptLanguage.ToString();
        if (!string.IsNullOrWhiteSpace(acceptLanguage))
        {
            foreach (var segment in acceptLanguage.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                var mappedCode = LanguageCultureNormalizer.MapAcceptLanguageSegment(segment);
                if (mappedCode is null)
                    continue;

                var language = await languageRegistry.GetByCodeAsync(mappedCode);
                if (language is { IsActive: true })
                    return language;
            }
        }

        var requestCulture = context.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture?.Name;
        if (!string.IsNullOrWhiteSpace(requestCulture))
        {
            var language = await languageRegistry.GetByCodeAsync(requestCulture);
            if (language is { IsActive: true })
                return language;
        }

        return await languageRegistry.GetDefaultAsync();
    }
}
